using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IPostgressHeroImportRepository
{
    Task BulkInsertHeroesAsync(IEnumerable<HeroImport> heroes);
    Task<IEnumerable<HeroImport>> GetHeroesBySeedIdAsync(Guid seedId);
}
