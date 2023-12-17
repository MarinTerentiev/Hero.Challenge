namespace MauiMobApp.Services;

public class MauiNavigationService : INavigationService
{
    public MauiNavigationService() { }

    public Task InitializeAsync() => NavigateToAsync("//HeroList");

    public Task NavigateToAsync(string route, IDictionary<string, object>? routeParameters = null)
    {
        var shellNavigation = new ShellNavigationState(route);

        return routeParameters != null
            ? Shell.Current.GoToAsync(shellNavigation, routeParameters)
            : Shell.Current.GoToAsync(shellNavigation);
    }
}
