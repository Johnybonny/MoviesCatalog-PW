using Pasternak.MoviesCatalog.MAUI.ViewModels;

namespace Pasternak.MoviesCatalog.MAUI;

public partial class DirectorsPage : ContentPage
{
	public DirectorsPage(DirectorsCollectionViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

        viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(viewModel.Directors))
            {
                var updatedProducers = viewModel.Directors;
                DirectorsList.ItemsSource = updatedProducers;
            }
        };
    }

    private void HandleDirectorItemTapped(object sender, ItemTappedEventArgs e)
    {
        var producerViewModel = (e.Item as DirectorViewModel).Clone() as DirectorViewModel;
        (BindingContext as DirectorsCollectionViewModel).EditDirector(producerViewModel);
    }
}