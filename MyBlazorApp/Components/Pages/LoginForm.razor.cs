using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor;
using MyBlazorApp.Models;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace MyBlazorApp.Components.Pages
{
    public partial class LoginForm
    {
        private readonly LoginFormModel _loginFormModel = new();

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        [Inject] private ProtectedSessionStorage? Pss { get; set; }

        [Inject] private ISnackbar Snackbar { get; set; } = null!;

        private async Task HandleSubmit()
        {
            const string apiUrl = "https://localhost:7049";
            try
            {
                var client = new RestClient(apiUrl, configureSerialization: ex => ex.UseNewtonsoftJson());
                var req = new RestRequest("/Auth/login", Method.Post).AddJsonBody(_loginFormModel);
                var res = await client.ExecuteAsync<LoginResponse>(req);

                if (res is { IsSuccessful: true, Data: not null })
                {
                    var token = res.Data.Token;
                    await Pss!.SetAsync("authToken", token);
                    NavigationManager.NavigateTo("/home", false);
                    Snackbar.Add("Login Successful", Severity.Success);
                    return;
                }
                
                Snackbar.Add("Login Failed", Severity.Error);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
            }

        }
    }
}