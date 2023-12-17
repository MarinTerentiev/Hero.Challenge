using CommunityToolkit.Mvvm.Input;
using MauiMobApp.Common;
using MauiMobApp.Models;
using MauiMobApp.Services;
using MauiMobApp.Services.Hero;

namespace MauiMobApp.ViewModels;

public partial class HeroAddViewModel : ViewModelBase
{
    private readonly IHeroService _heroService;
    HeroModel heroModel = new HeroModel();

    public string Name
    {
        get => heroModel.Name;
        set
        {
            if (heroModel.Name != value)
            {
                heroModel.Name = value;
                OnPropertyChanged("Name");
            }
        }
    }

    public string Class
    {
        get => heroModel.Class;
        set
        {
            if (heroModel.Class != value)
            {
                heroModel.Class = value;
                OnPropertyChanged("Class");
            }
        }
    }

    public string Story
    {
        get => heroModel.Story;
        set
        {
            if (heroModel.Story != value)
            {
                heroModel.Story = value;
                OnPropertyChanged("Story");
            }
        }
    }

    public Weapon SelectedWeapon
    {
        get => heroModel.Weapon;
        set
        {
            if (heroModel.Weapon != value)
            {
                heroModel.Weapon = value;
                OnPropertyChanged("SelectedWeapon");
            }
        }
    }

    public List<string> Weapons
    {
        get
        {
            return Enum.GetNames(typeof(Weapon)).Select(b => b.SplitCamelCase()).ToList();
        }
    }

    public HeroAddViewModel(INavigationService navigationService, IHeroService heroService
        ) : base(navigationService)
    {
        _heroService = heroService;
    }
    
    [RelayCommand]
    public async Task SaveNewNero()
    {
        if (!IsValid()) return;

        await IsBusyFor(async () =>
        {
            var newHero = new HeroModel
            {
                Class = Class,
                Name = Name,
                Story = Story,
                Weapon = SelectedWeapon
            };

            var result = await _heroService.AddHeroAsync(newHero);
            if (result)
            {
                await NavigationService.NavigateToAsync("//HeroList");
            }
        });
    }

    [RelayCommand]
    public async Task BackAsync()
    {
        await NavigationService.NavigateToAsync("//HeroList");
    }

    private bool IsValid()
    {
        if (string.IsNullOrEmpty(Name)) return false;
        if (string.IsNullOrEmpty(Story)) return false;
        if (string.IsNullOrEmpty(Class)) return false;
        return true;
    }
}
