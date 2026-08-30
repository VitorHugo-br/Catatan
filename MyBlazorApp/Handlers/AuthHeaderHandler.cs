using Microsoft.AspNetCore.Authentication.JwtBearer;
using MyBlazorApp.Models.DTO;
using MyBlazorApp.Services;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MyBlazorApp.Handlers;

public class AuthHeaderHandler(TokenProvider tokenProvider) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(tokenProvider.Jwt))
        {
            var token = JsonSerializer.Deserialize<LoginResponse>(tokenProvider.Jwt);
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token?.token);
        }
        return base.SendAsync(request, cancellationToken);
    }
}
