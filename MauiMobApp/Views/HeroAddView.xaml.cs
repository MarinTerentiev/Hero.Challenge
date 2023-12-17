using MauiMobApp.ViewModels;
namespace MauiMobApp.Views;

public partial class HeroAddView : ContentPage
{
    public HeroAddView(HeroAddViewModel viewModel)
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