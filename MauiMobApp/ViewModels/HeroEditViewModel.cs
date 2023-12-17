using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMobApp.Common;
using MauiMobApp.Models;
using MauiMobApp.Services;
using MauiMobApp.Services.Hero;

namespace MauiMobApp.ViewModels;

[QueryProperty(nameof(HeroId), "Id")]
public partial class HeroEditViewModel : ViewModelBase
{
    private readonly IHeroService _heroService;
    public HeroModel Hero { get; set; } = new HeroModel();

    [ObservableProperty]
    private int _heroId;

    public string Name => Hero.Name;

    public string Class => Hero.Class;

    public string Story
    {
        get => Hero.Story;
        set
        {
            if (Hero.Story != value)
            {
                Hero.Story = value;
                OnPropertyChanged("Story");
            }
        }
    }
    
    public Weapon SelectedWeapon
    {
        get => Hero.Weapon;
        set
        {
            if (Hero.Weapon != value)
            {
                Hero.Weapon = value;
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

    public HeroEditViewModel(INavigationService navigationService, IHeroService heroService
        ) : base(navigationService)
    {
        _heroService = heroService;
    }

    public override async Task InitializeAsync()
    {
        await IsBusyFor(async () =>
        {
            var heroModel = await _heroService.GetHeroByIdAsync(HeroId);
            if (heroModel == null)
            {
                await BackAsync();
                return;
            }

            Hero = heroModel;
            OnPropertyChanged("Name");
            OnPropertyChanged("Class");
            OnPropertyChanged("Story");
            OnPropertyChanged("SelectedWeapon");
        });
    }

    [RelayCommand]
    public async Task SaveNero()
    {
        if (!IsValid()) return;

        await IsBusyFor(async () =>
        {
            var hero = new HeroModel
            {
                Id = HeroId,
                Class = Hero.Class,
                Name = Hero.Name,
                Story = Story,
                Weapon = SelectedWeapon
            };

            var result = await _heroService.UpdateHeroAsync(hero);
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
        if (string.IsNullOrEmpty(Story)) return false;
        return true;
    }
}
