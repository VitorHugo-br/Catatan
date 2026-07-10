using MyBlazorApp.Handlers;
using MyBlazorApp.interfaces;
using MyBlazorApp.Services;

namespace MyBlazorApp.Extensions;

public static class HttpClientConfigExtension
{
    public static IServiceCollection AddHttpClientConfig(this IServiceCollection services)
    {

        services.AddScoped<AuthHeaderHandler>();

        services.AddHttpClient<IChamadoService, ChamadoService>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7049/");
        }).AddHttpMessageHandler<AuthHeaderHandler>();
        return services;
    }
}