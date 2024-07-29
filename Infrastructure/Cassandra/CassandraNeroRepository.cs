using Application.Common.Interfaces;
using Cassandra;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Options;

namespace Infrastructure.CassandraRepository;

public class CassandraNeroRepository : ICassandraNeroRepository
{
    private readonly ISession _session;

    public CassandraNeroRepository(IOptions<CassandraSettings> options, CassandraSettings cassandraSettings)
    {
        var settings = options.Value;
        var cluster = Cluster.Builder()
            .AddContactPoint(settings.ContactPoints)
            .WithPort(settings.Port)
            .WithCredentials(settings.Username, settings.Password)
            .Build();

        _session = cluster.Connect(settings.Keyspace);
    }

    public async Task AddAsync(Hero hero)
    {
        var query = "INSERT INTO hero (id, name, class, story, weapon) VALUES (?, ?, ?, ?, ?)";
        await _session.ExecuteAsync(new SimpleStatement(query, hero.Id, hero.Name, hero.Class, hero.Story, (int)hero.Weapon));
    }

    public async Task DeleteAsync(Guid id)
    {
        var query = "DELETE FROM hero WHERE id = ?";
        await _session.ExecuteAsync(new SimpleStatement(query, id));
    }

    public async Task<IEnumerable<Hero>> GetAllAsync()
    {
        var query = "SELECT * FROM hero";
        var result = await _session.ExecuteAsync(new SimpleStatement(query));

        var ret = result.Select(CreateNero);

        return ret;
    }

    public async Task<Hero?> GetByIdAsync(Guid id)
    {
        var query = "SELECT * FROM hero WHERE id = ?";
        var result = await _session.ExecuteAsync(new SimpleStatement(query, id));

        var row = result.FirstOrDefault();
        if (row == null)
        {
            return null;
        }

        var ret = CreateNero(row);

        return ret;
    }

    public async Task UpdateAsync(Hero hero)
    {
        var query = "UPDATE hero SET name = ?, class = ?, story = ?, weapon = ? WHERE id = ?";
        await _session.ExecuteAsync(new SimpleStatement(query, hero.Name, hero.Class, hero.Story, (int)hero.Weapon, hero.Id));
    }

    private Hero CreateNero(Row row)
    {
        var ret = new Hero
        {
            Id = row.GetValue<Guid>("id"),
            Name = row.GetValue<string>("name"),
            Class = row.GetValue<string>("class"),
            Story = row.GetValue<string>("story"),
            Weapon = Enum.TryParse(row.GetValue<int>("weapon").ToString(), out Weapon myWeapon) ? myWeapon : Weapon.None,
        };

        return ret;
    }
}
