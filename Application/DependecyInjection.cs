using Application.Common.Behaviours;
using Application.RabbitmqPublisher;
using Domain.Common;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class DependecyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });

        services.Configure<RabbitmqSettings>(opt => configuration.GetSection("RabbitmqSettings").Bind(opt));
        services.AddSingleton<IHeroPublisher, HeroRabbitmqPublisher>();
        services.AddSingleton<ITextPublisher, TextRabbitmqPublisher>();

        return services;
    }
}
