using MyBlazorApp.interfaces;
using MyBlazorApp.Services;

namespace MyBlazorApp.Extensions;

public static class HttpClientConfigExtension
{
    public static IServiceCollection AddHttpClientConfig(this IServiceCollection services)
    {
        services.AddHttpClient<IMyTaskService, MyTaskService>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7049/");
        });
        return services;
    }
}