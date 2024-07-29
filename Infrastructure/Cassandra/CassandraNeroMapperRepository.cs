using Application.Common.Interfaces;
using Cassandra;
using Cassandra.Mapping;
using Domain.Entities;

namespace Infrastructure.CassandraRepository;

public class CassandraNeroMapperRepository : ICassandraNeroRepository
{
    private readonly ISession _session;
    private readonly IMapper _mapper;

    public CassandraNeroMapperRepository(ISession session)
    {
        _session = session;
        _mapper = new Mapper(_session);
    }

    public async Task AddAsync(Hero hero)
    {
        await _mapper.InsertAsync(hero);
    }

    public async Task DeleteAsync(Guid id)
    {
        var hero = await _mapper.SingleOrDefaultAsync<Hero>("WHERE id = ?", id);
        if (hero != null)
        {
            await _mapper.DeleteAsync(hero);
        }
    }

    public async Task<IEnumerable<Hero>> GetAllAsync()
    {
        var ret = await _mapper.FetchAsync<Hero>();
        return ret;
    }

    public async Task<Hero?> GetByIdAsync(Guid id)
    {
        var hero = await _mapper.SingleOrDefaultAsync<Hero>("WHERE id = ?", id);
        return hero;
    }

    public async Task UpdateAsync(Hero hero)
    {
        await _mapper.UpdateAsync(hero);
    }
}
