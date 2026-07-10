using MyBlazorApp.Models.DTO;
using MyBlazorApp.Services;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MyBlazorApp.Handlers;

public class AuthHeaderHandler(TokenProvider tokenProvider) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.WriteLine("[AuthHeaderHandler] SendAsync chamado");
        if (!string.IsNullOrEmpty(tokenProvider.Jwt))
        {
            var token = JsonSerializer.Deserialize<LoginResponse>(tokenProvider.Jwt);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token?.token);
        }
        Console.WriteLine($"[AuthHeaderHandler] {request.Method} {request.RequestUri} -> Authorization: {request.Headers.Authorization?.ToString() ?? "AUSENTE"}");
        return base.SendAsync(request, cancellationToken);
    }
}
