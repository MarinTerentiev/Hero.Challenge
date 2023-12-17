using MauiMobApp.ViewModels;

namespace MauiMobApp.Views;

public partial class HeroListView : ContentPage
{
	public HeroListView(HeroListViewModel viewModel)
	{
		BindingContext = viewModel;
        InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is not IViewModelBase ivmb)
        {
            return;
        }

        await ivmb.InitializeAsyncCommand.ExecuteAsync(null);
    }
}