using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ICassandraNeroRepository
{
    Task<Hero?> GetByIdAsync(int id);
    Task<IEnumerable<Hero>> GetAllAsync();
    Task AddAsync(Hero hero);
    Task UpdateAsync(Hero hero);
    Task DeleteAsync(int id);
}
