using CommunityToolkit.Mvvm.Input;
using MauiMobApp.Services;

namespace MauiMobApp.ViewModels;

internal interface IViewModelBase : IQueryAttributable
{
    public INavigationService NavigationService { get; }

    public IAsyncRelayCommand InitializeAsyncCommand { get; }

    public bool IsBusy { get; }

    public bool IsInitialized { get; }

    Task InitializeAsync();
}
