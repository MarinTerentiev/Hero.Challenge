using MauiMobApp.Services;
using MauiMobApp.Services.Hero;
using MauiMobApp.ViewModels;
using MauiMobApp.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;

namespace MauiMobApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterAppServices()
            .RegisterViewModels()
            .RegisterViews();

        return builder.Build();
    }

    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<INavigationService, MauiNavigationService>();
        mauiAppBuilder.Services.AddSingleton<IHeroService, HeroMockService>();

#if DEBUG
        mauiAppBuilder.Logging.AddDebug();
#endif

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<HeroListViewModel>();
        mauiAppBuilder.Services.AddTransient<HeroEditViewModel>();
        mauiAppBuilder.Services.AddTransient<HeroAddViewModel>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<HeroListView>();
        mauiAppBuilder.Services.AddTransient<HeroAddView>();
        mauiAppBuilder.Services.AddTransient<HeroEditView>();

        return mauiAppBuilder;
    }

}
