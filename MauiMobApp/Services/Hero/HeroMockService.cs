using MauiMobApp.Models;

namespace MauiMobApp.Services.Hero;

public class HeroMockService : IHeroService
{
    private List<HeroModel> MockHeroies = new List<HeroModel>
    {
        new HeroModel
        {
            Id = 1,
            Class = "Warrior",
            Name = "Joy Doe",
            Story = "Simple Warrior",
            Weapon = Weapon.Sword
        },
        new HeroModel
        {
            Id = 2,
            Class = "Archer",
            Name = "Max Doe",
            Story = "Simple Archer",
            Weapon = Weapon.Bow
        },
        new HeroModel
        {
            Id = 3,
            Class = "Warrior",
            Name = "Jo Doe",
            Story = "Base Warrior",
            Weapon = Weapon.Sword
        },
        new HeroModel
        {
            Id = 4,
            Class = "Archer",
            Name = "Ma Doe",
            Story = "Base Archer",
            Weapon = Weapon.Bow
        }
    };

    public async Task<List<HeroModel>> GetHeroesAsync()
    {
        await Task.Delay(100);
        return MockHeroies;
    }

    public async Task<HeroModel?> GetHeroByIdAsync(int id)
    {
        await Task.Delay(100);
        return MockHeroies.FirstOrDefault(x => x.Id == id);
    }

    public async Task<bool> AddHeroAsync(HeroModel model)
    {
        await Task.Delay(100);
        model.Id = MockHeroies.Max(x => x.Id) + 1;
        MockHeroies.Add(model);

        return true;
    }

    public async Task<bool> RemoveHeroAsync(int id)
    {
        await Task.Delay(100);
        var entry = await GetHeroByIdAsync(id);
        if (entry != null)
        {
            MockHeroies.Remove(entry);
        }
            
        return true;
    }

    public async Task<bool> UpdateHeroAsync(HeroModel model)
    {
        var entry = await GetHeroByIdAsync(model.Id);
        if (entry != null)
        {
            entry.Story = model.Story;
            entry.Weapon = model.Weapon;
        }

        return true;
    }
}
