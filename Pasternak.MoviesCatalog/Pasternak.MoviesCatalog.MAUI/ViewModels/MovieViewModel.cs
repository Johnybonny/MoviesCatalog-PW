using CommunityToolkit.Mvvm.ComponentModel;
using Pasternak.MoviesCatalog.Core;
using Pasternak.MoviesCatalog.Interfaces;

namespace Pasternak.MoviesCatalog.MAUI.ViewModels
{
    public partial class MovieViewModel : ObservableObject, IMovie
    {
        [ObservableProperty]
        private string iD;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private int releaseYear;

        [ObservableProperty]
        private Genre movieGenre;

        [ObservableProperty]
        private IDirector director;

        public IReadOnlyList<string> AllGenres { get; } = Enum.GetNames(typeof(Genre));

        public MovieViewModel( IMovie movie)
        {
            ID = movie.ID;
            Title = movie.Title;
            ReleaseYear = movie.ReleaseYear;
            MovieGenre = movie.MovieGenre;
            Director = movie.Director;
        }

        public MovieViewModel()
        {
        }

        public object Clone()
        {
            return new MovieViewModel(this);
        }
    }
}
