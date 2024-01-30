using Pasternak.MoviesCatalog.MAUI.ViewModels;

namespace Pasternak.MoviesCatalog.MAUI;

public partial class MoviesPage : ContentPage
{
	public MoviesPage(MoviesCollectionViewModel viewModel)
	{
        InitializeComponent();
        BindingContext = viewModel;

        viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(viewModel.Movies))
            {
                var updatedMovies = viewModel.Movies;
                MoviesList.ItemsSource = updatedMovies;
            }
        };
    }

    private void HandleMovieItemTapped(object sender, ItemTappedEventArgs e)
    {
        var movieViewModel = (e.Item as MovieViewModel).Clone() as MovieViewModel;
        (BindingContext as MoviesCollectionViewModel).EditMovie(movieViewModel);
    }
}