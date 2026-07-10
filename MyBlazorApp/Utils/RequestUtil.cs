using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MyBlazorApp.interfaces;
using MyBlazorApp.Models.DTO;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;
using System.Text.Json;

namespace MyBlazorApp.Utils;

public class RequestUtil(ILocalStorageService service)
{

    public async Task<RestClient?> ConfigAuthorizationBeforeRequest()
    {
        const string apiUrl = "https://localhost:7049";
        var token = await GetTokenFromSessionStorage();
        if (token is null) return null;
        var authenticator = new JwtAuthenticator(token);
        var clientOptions = new RestClientOptions(apiUrl) { Authenticator = authenticator };
        var restClient = new RestClient(clientOptions, configureSerialization: c => c.UseNewtonsoftJson());
        return restClient;
    }

    public async Task<string?> GetTokenFromSessionStorage()
    {
        var token = await service.GetItemAsync("authToken");
        if (token is null) return null;
        var result = JsonSerializer.Deserialize<LoginResponse>(token);
        return result?.token ?? null;
    }
}