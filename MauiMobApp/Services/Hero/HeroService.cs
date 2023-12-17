using MauiMobApp.Models;

namespace MauiMobApp.Services.Hero;

internal class HeroService : IHeroService
{
    public Task<List<HeroModel>> GetHeroesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<HeroModel?> GetHeroByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AddHeroAsync(HeroModel model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveHeroAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateHeroAsync(HeroModel model)
    {
        throw new NotImplementedException();
    }
}
