using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pasternak.MoviesCatalog.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pasternak.MoviesCatalog.MAUI.ViewModels
{
    public partial class DirectorsCollectionViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<DirectorViewModel> directors;

        private BLC.BLC _blc;

        [ObservableProperty]
        private DirectorViewModel directorEdit;

        [ObservableProperty]
        private bool isCreating = false;

        [ObservableProperty]
        private bool isEditing = false;

        public ICommand CancelCommand { get; set; }

        [ObservableProperty]
        public string filterPhrase;

        [ObservableProperty]
        public string filterType;

        public ICommand SearchCommand { get; set; }

        public ICommand ClearFilterCommand { get; }

        public DirectorsCollectionViewModel(BLC.BLC blc)
        {
            _blc = blc;
            directors = new ObservableCollection<DirectorViewModel>();

            foreach (var director in blc.GetDirectors())
            {
                directors.Add(new DirectorViewModel(director));
            }

            DirectorEdit = null;

            CancelCommand = new Command(
                execute: () =>
                {
                    DirectorEdit.PropertyChanged -= OnDirectorEditPropertyChanged;
                    DirectorEdit = null;
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

                   FilterDirectors();
               });

            FilterType = "Name";
            FilterPhrase = string.Empty;
            ClearFilterCommand = new Command(
                execute: () =>
                {
                    FilterType = "Name";
                    FilterPhrase = string.Empty;
                    FilterDirectors();
                }
            );

        }

        private void FilterDirectors()
        {
            RefreshDirectors();

            IEnumerable<DirectorViewModel> filteredDirectors = directors;

            if (!string.IsNullOrEmpty(FilterType) && !string.IsNullOrEmpty(FilterPhrase))
            {
                switch (FilterType)
                {
                    case "Age":
                        filteredDirectors = Directors.Where(m => m.Age.ToString() == FilterPhrase);
                        break;
                    case "Nationality":
                        filteredDirectors = Directors.Where(m => m.Nationality.Contains(FilterPhrase, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "Name":
                    default:
                        filteredDirectors = Directors.Where(m => m.Name.Contains(FilterPhrase, StringComparison.OrdinalIgnoreCase));
                        break;
                }
            }

            Directors = new ObservableCollection<DirectorViewModel>(filteredDirectors);
        }

        [RelayCommand(CanExecute = nameof(CanCreateNewDirector))]
        private void CreateNewDirector()
        {
            DirectorEdit = new DirectorViewModel();
            IsCreating = true;
            DirectorEdit.PropertyChanged += OnDirectorEditPropertyChanged;
            RefreshCanExecute();
        }

        private bool CanCreateNewDirector()
        {
            return !IsEditing;
        }

        [RelayCommand(CanExecute = nameof(CanEditDirectorBeSaved))]
        private void SaveDirector()
        {
            if (IsCreating)
            {
                _blc.CreateNewDirector(DirectorEdit.Name, DirectorEdit.Age, DirectorEdit.Nationality);
            }
            else
            {
                _blc.EditDirector(DirectorEdit.ID, DirectorEdit.Name, DirectorEdit.Age, DirectorEdit.Nationality);
            }

            DirectorEdit.PropertyChanged -= OnDirectorEditPropertyChanged;
            DirectorEdit = null;
            IsEditing = false;
            IsCreating = false;
            RefreshCanExecute();
            RefreshDirectors();
        }

        private bool CanEditDirectorBeSaved()
        {
            return DirectorEdit != null &&
                DirectorEdit.Name != null &&
                DirectorEdit.Name.Length > 1 &&
                DirectorEdit.Age > 0 &&
                DirectorEdit.Age < 120 &&
                DirectorEdit.Nationality != null &&
                DirectorEdit.Nationality.Length > 0;
        }

        public void EditDirector(DirectorViewModel director)
        {
            DirectorEdit = director;
            DirectorEdit.PropertyChanged += OnDirectorEditPropertyChanged;
            IsEditing = true;
            IsCreating = false;
            RefreshCanExecute();
        }

        [RelayCommand(CanExecute = nameof(CanEditDirectorBeSaved))]
        public void DeleteDirector()
        {
            _blc.DeleteDirector(DirectorEdit.ID);
            isCreating = false;
            IsEditing = false;
            DirectorEdit = null;
            RefreshCanExecute();
            RefreshDirectors();
        }

        private void OnDirectorEditPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            SaveDirectorCommand.NotifyCanExecuteChanged();
        }

        private void RefreshCanExecute()
        {
            CreateNewDirectorCommand.NotifyCanExecuteChanged();
            SaveDirectorCommand.NotifyCanExecuteChanged();
            DeleteDirectorCommand.NotifyCanExecuteChanged();
            (CancelCommand as Command).ChangeCanExecute();
        }

        void RefreshDirectors()
        {
            if (Directors == null)
            {
                Directors = new ObservableCollection<DirectorViewModel>();
            }
            Directors.Clear();
            foreach (IDirector director in _blc.GetDirectors())
            {
                Directors.Add(new DirectorViewModel(director));
            }
            OnPropertyChanged(nameof(Directors));
        }

    }
}
