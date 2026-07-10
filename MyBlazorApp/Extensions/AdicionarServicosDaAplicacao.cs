using MyBlazorApp.interfaces;
using MyBlazorApp.Services;
using MyBlazorApp.Utils;

namespace MyBlazorApp.Extensions;

public static class AdicionarServicosDaAplicacao
{
    public static IServiceCollection AdicionarServicos(this IServiceCollection services)
    {
        services.AddScoped<RequestUtil>();
        services.AddScoped<ILocalStorageService, LocalStorageService>();
        services.AddSingleton<IChamadoTimerService, ChamadoTimerService>();
        return services;
    }
}
