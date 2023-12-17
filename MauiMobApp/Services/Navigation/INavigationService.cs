namespace MauiMobApp.Services;

public interface INavigationService
{
    Task InitializeAsync();

    Task NavigateToAsync(string route, IDictionary<string, object>? routeParameters = null);
}

