using Application.Common.Interfaces;
using Cassandra.Mapping;
using Cassandra;
using Domain.Common;
using Infrastructure.CassandraRepository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("Default");
        services.AddDbContext<HeroEfDbContext>(opt => opt.UseSqlServer(cs));

        //for CassandraNeroRepository
        services.Configure<CassandraSettings>(opt => configuration.GetSection("CassandraSettings").Bind(opt));
        //for CassandraNeroMapperRepository
        var cassandraSettings = new CassandraSettings();
        configuration.GetSection("CassandraSettings").Bind(cassandraSettings);
        services.AddSingleton(cassandraSettings);
        services.AddSingleton<ISession>(sp =>
        {
            var settings = sp.GetRequiredService<CassandraSettings>();
            var cluster = Cluster.Builder()
                .AddContactPoints(settings.ContactPoints)
                .WithPort(settings.Port)
                .WithCredentials(settings.Username, settings.Password)
                .Build();
            var session = cluster.Connect(settings.Keyspace);

            // Register mappings
            MappingConfiguration.Global.Define<CassandraMappings>();

            return session;
        });

        services.AddSingleton<ICassandraNeroRepository, CassandraNeroRepository>();

        return services;
    }
}
