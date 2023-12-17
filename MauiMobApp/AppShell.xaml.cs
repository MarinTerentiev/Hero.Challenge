using MauiMobApp.Views;

namespace MauiMobApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        AppShell.InitializeRouting();
        InitializeComponent();
    }

    private static void InitializeRouting()
    {
        Routing.RegisterRoute("HeroList", typeof(HeroListView));
        Routing.RegisterRoute("HeroAdd", typeof(HeroAddView));
        Routing.RegisterRoute("HeroEdit", typeof(HeroEditView));
    }
}
