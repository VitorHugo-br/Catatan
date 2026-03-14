using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;

namespace MyBlazorApp.Utils;

public class RequestUtil(ProtectedSessionStorage protectedSessionStorage, NavigationManager nav)
{
    private ProtectedSessionStorage Pss { get; set; } = protectedSessionStorage;

    public async Task<RestClient?> ConfigAuthorizationBeforeRequest()
    {
        const string apiUrl = "https://localhost:7049";
        var token = await GetTokenFromSessionStorage();
        if (token.Value is null)
        {
            nav.NavigateTo("/", false);
            return null;
        };
        var authenticator = new JwtAuthenticator(token.Value);
        var clientOptions = new RestClientOptions(apiUrl) { Authenticator = authenticator };
        var restClient = new RestClient(clientOptions, configureSerialization: c => c.UseNewtonsoftJson());
        return restClient;
    }

    public async Task<ProtectedBrowserStorageResult<string>> GetTokenFromSessionStorage()
    {
        var token = await Pss.GetAsync<string>("authToken");
        return token;
    }
}