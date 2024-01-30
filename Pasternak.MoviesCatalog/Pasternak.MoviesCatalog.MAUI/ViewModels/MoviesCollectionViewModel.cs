using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Pasternak.MoviesCatalog.BLC;
using Pasternak.MoviesCatalog.Core;
using Pasternak.MoviesCatalog.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pasternak.MoviesCatalog.MAUI.ViewModels
{
    public partial class MoviesCollectionViewModel: ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<MovieViewModel> movies;

        private ObservableCollection<IDirector> allDirectors;
        public ObservableCollection<IDirector> AllDirectors
        {
            get { return allDirectors; }
            set
            {
                if (value != allDirectors)
                {
                    allDirectors = value;
                    OnPropertyChanged(nameof(AllDirectors));
                }
            }
        }

        private BLC.BLC _blc;

        [ObservableProperty]
        private MovieViewModel movieEdit;

        [ObservableProperty]
        private bool isEditing = false;

        [ObservableProperty]
        private bool isCreating = false;

        public ICommand CancelCommand { get; set; }

        [ObservableProperty]
        public string filterPhrase;

        [ObservableProperty]
        public string filterType;

        public ICommand SearchCommand { get; set; }

        public ICommand ClearFilterCommand { get; }

        public MoviesCollectionViewModel(BLC.BLC blc)
        {
            _blc = blc;
            movies = new ObservableCollection<MovieViewModel>();

            foreach(var movie in  blc.GetMovies()) 
            {
                movies.Add(new MovieViewModel(movie));
            }

            RefreshDirectors();

            MovieEdit = null;

            CancelCommand = new Command(
                execute: () =>
                {
                    MovieEdit.PropertyChanged -= OnMovieEditPropertyChanged;
                    MovieEdit = null;
                    IsEditing = false;
                    IsCreating = false;
                    RefreshCanExecute();
                },
                canExecute: () => IsEditing || IsCreating
                );

            SearchCommand = new Command(
               execute: () =>
               {
                   if (IsEditing || IsCreating)
                   {
                       CancelCommand.Execute(null);
                   }

                   FilterMovies();
               });

            FilterType = "Title";
            FilterPhrase = string.Empty;
            ClearFilterCommand = new Command(
                execute: () =>
                {
                    FilterType = "Title";
                    FilterPhrase = string.Empty;
                    FilterMovies();
                }
            );
        }

        private void FilterMovies()
        {
            RefreshMovies();

            IEnumerable<MovieViewModel> filteredMovies = movies;

            if (!string.IsNullOrEmpty(FilterType) && !string.IsNullOrEmpty(FilterPhrase))
            {
                switch (FilterType)
                {
                    case "ReleaseYear":
                        filteredMovies = Movies.Where(m => m.ReleaseYear.ToString() == FilterPhrase);
                        break;
                    case "Title":
                        filteredMovies = Movies.Where(m => m.Title.Contains(FilterPhrase, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "MovieGenre":
                    default:
                        filteredMovies = Movies.Where(m => m.MovieGenre.ToString().Contains(FilterPhrase, StringComparison.OrdinalIgnoreCase));
                        break;
                }
            }

            Movies = new ObservableCollection<MovieViewModel>(filteredMovies);
        }

        [RelayCommand(CanExecute =nameof(CanCreateNewMovie))]
        private void CreateNewMovie()
        {
            RefreshDirectors();
            MovieEdit = new MovieViewModel();
            if (allDirectors.Count() > 0)
            {
                MovieEdit.Director = allDirectors.ElementAt(0);
            }
            else
            {
                return;
            }
            IsCreating = true;
            MovieEdit.PropertyChanged += OnMovieEditPropertyChanged;
            RefreshCanExecute();
        }

        private bool CanCreateNewMovie()
        {
            return !IsEditing;
        }

        [RelayCommand(CanExecute =nameof(CanEditMovieBeSaved))]
        private void SaveMovie()
        {
            if (IsCreating)
            {
                _blc.CreateNewMovie(MovieEdit.Title,
                    MovieEdit.ReleaseYear,
                    MovieEdit.MovieGenre,
                    MovieEdit.Director.ID);
            }
            else
            {
                _blc.EditMovie(MovieEdit.ID,
                    MovieEdit.Title,
                    MovieEdit.ReleaseYear,
                    MovieEdit.MovieGenre,
                    MovieEdit.Director.ID);
            }
            MovieEdit.PropertyChanged -= OnMovieEditPropertyChanged;
            MovieEdit = null;
            IsEditing = false;
            IsCreating = false;
            RefreshCanExecute();
            RefreshMovies();
        }

        private bool CanEditMovieBeSaved()
        {
            return MovieEdit != null &&
                MovieEdit.Title != null &&
                MovieEdit.Title.Length > 1 &&
                MovieEdit.ReleaseYear > 1880;
        }

        public void EditMovie(MovieViewModel movie)
        {
            MovieEdit = movie;
            MovieEdit.PropertyChanged += OnMovieEditPropertyChanged;
            IsEditing = true;
            IsCreating = false;
            RefreshCanExecute();
        }

        [RelayCommand(CanExecute = nameof(CanEditMovieBeSaved))]
        public void DeleteMovie()
        {
            _blc.DeleteMovie(MovieEdit.ID);
            IsCreating = false;
            IsEditing = false;
            MovieEdit = null;
            RefreshCanExecute();
            RefreshMovies();
        }

        private void OnMovieEditPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            SaveMovieCommand.NotifyCanExecuteChanged();
        }

        private void RefreshCanExecute()
        {
            CreateNewMovieCommand.NotifyCanExecuteChanged();
            SaveMovieCommand.NotifyCanExecuteChanged();
            DeleteMovieCommand.NotifyCanExecuteChanged();
            (CancelCommand as Command).ChangeCanExecute();
        }

        public void RefreshMovies()
        {
            if (Movies == null)
            {
                Movies = new ObservableCollection<MovieViewModel>();
            }
            Movies.Clear();
            foreach (IMovie movie in _blc.GetMovies())
            {
                Movies.Add(new MovieViewModel(movie));
            }
        }

        public void RefreshDirectors()
        {
            allDirectors = new ObservableCollection<IDirector>(_blc.GetDirectors());
            OnPropertyChanged(nameof(AllDirectors));
        }

    }

    public class MyEnumToIntConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            Genre genre = (Genre)value;
            int result = (int)genre;
            return result;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            int val = (int)value;
            if (val != -1)
                return (Genre)value;
            else
                return 0;

        }
    }
}
