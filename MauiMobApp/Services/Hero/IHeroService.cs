using MauiMobApp.Models;

namespace MauiMobApp.Services.Hero;

public interface IHeroService
{
    Task<List<HeroModel>> GetHeroesAsync();
    Task<HeroModel?> GetHeroByIdAsync(int id);

    Task<bool> AddHeroAsync(HeroModel model);
    Task<bool> RemoveHeroAsync(int id);
    Task<bool> UpdateHeroAsync(HeroModel model);
}
