using Application.Common.Interfaces;
using Dapper;
using Domain.Common;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Infrastructure.PostgresRepository;

public class PostgressHeroImportRepository : IPostgressHeroImportRepository
{
    private readonly string _connectionString;

    public PostgressHeroImportRepository(IOptions<PostgresSettings> options)
    {
        _connectionString = options.Value.ConnectionStrings;
    }

    public async Task BulkInsertHeroesAsync(IEnumerable<HeroImport> heroes)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            var sql = @"
                INSERT INTO herochallenge.public.heroimport (name, class, story, weapon, seedId)
                VALUES (@Name, @Class, @Story, @Weapon, @SeedId)";

            await connection.ExecuteAsync(sql, heroes);
        }
    }

    public async Task<IEnumerable<HeroImport>> GetHeroesBySeedIdAsync(Guid seedId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            var sql = @"
                SELECT id, name, class, story, weapon, seedId 
                FROM herochallenge.public.heroimport 
                WHERE seedId = @SeedId";

            return await connection.QueryAsync<HeroImport>(sql, new { SeedId = seedId });
        }
    }
}
