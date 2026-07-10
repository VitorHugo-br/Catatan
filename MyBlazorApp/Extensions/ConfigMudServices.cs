using MudBlazor;
using MudBlazor.Services;

namespace MyBlazorApp.Extensions;

public static class ConfigMudServices
{
    public static IServiceCollection AdicionarMudServices(this IServiceCollection services)
    {
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            config.SnackbarConfiguration.PreventDuplicates = true;
        });
        return services;
    }
}
