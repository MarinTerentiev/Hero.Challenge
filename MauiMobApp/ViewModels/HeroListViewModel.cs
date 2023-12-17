using CommunityToolkit.Mvvm.Input;
using MauiMobApp.Models;
using MauiMobApp.Services;
using MauiMobApp.Services.Hero;
using System.Collections.ObjectModel;

namespace MauiMobApp.ViewModels;

public partial class HeroListViewModel : ViewModelBase
{
    private readonly IHeroService _heroService;

    public ObservableCollection<HeroModel> HeroModels { get; set; } = new();

    public HeroListViewModel(INavigationService navigationService, IHeroService heroService
        ) : base(navigationService)
    {
        _heroService = heroService;
    }

    public override async Task InitializeAsync()
    {
        await IsBusyFor(async () =>
        {
            await GetHeroes();
        });
    }

    private async Task GetHeroes()
    {
        OnPropertyChanged("HeroModels");
        var heroes = await _heroService.GetHeroesAsync();
        HeroModels = new ObservableCollection<HeroModel>(heroes);
        OnPropertyChanged("HeroModels");
    }

    [RelayCommand]
    public async Task AddNeroAsync()
    {
        await NavigationService.NavigateToAsync("HeroAdd");
    }

    [RelayCommand]
    public async Task EditNeroAsync(object? args)
    {
        if (args is ListView list)
        {
            if (list.SelectedItem is HeroModel heroModel)
            {
                list.SelectedItem = null;
                await NavigationService.NavigateToAsync("HeroEdit",
                    new Dictionary<string, object> { { nameof(heroModel.Id), heroModel.Id } });
                
            }
        }
        
    }

    [RelayCommand]
    public async Task RemoveNero(object? args)
    {
        if (args is ListView list)
        {
            if (list.SelectedItem is HeroModel heroModel)
            {
                await IsBusyFor(async () =>
                {
                    await Task.Delay(100);
                    await _heroService.RemoveHeroAsync(heroModel.Id);
                    list.SelectedItem = null;
                    await GetHeroes();
                });

            }
        }
    }
}
