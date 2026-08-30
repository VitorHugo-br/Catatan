using Microsoft.AspNetCore.Components;
using System.Net;

namespace MyBlazorApp.Handlers;

public class UnauthorizedRedirectHandler(ILogger<UnauthorizedRedirectHandler> logger) : DelegatingHandler
{
    [Inject]
    public NavigationManager _navigation { get; set; } = null!;
    private readonly ILogger<UnauthorizedRedirectHandler> _logger = logger;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogWarning("Token expirado ou inválido ao chamar {Url}", request.RequestUri);

            _navigation.NavigateTo("/", forceLoad: true);
        }

        return response;
    }
}
