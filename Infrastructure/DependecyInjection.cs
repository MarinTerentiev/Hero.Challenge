using Application.Common.Interfaces;
using Domain.Common;
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

        services.Configure<CassandraSettings>(configuration.GetSection(nameof(CassandraSettings)));
        services.AddSingleton<ICassandraNeroRepository>();

        return services;
    }
}
