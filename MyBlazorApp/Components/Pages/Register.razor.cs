using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyBlazorApp.Models;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace MyBlazorApp.Components.Pages
{
    public partial class Register
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private ISnackbar? Snackbar { get; set; }

        private readonly RegisterFormModel _registerFormModel = new RegisterFormModel();

        private async Task HandleSubmit()
        {
            Dictionary<string, string> requestAdapter = new()
            {
                { "name", _registerFormModel.Name },
                { "email", _registerFormModel.Email },
                { "password", _registerFormModel.Password }
            };

            const string apiUrl = "https://localhost:7049";
            var client = new RestClient(apiUrl, configureSerialization: ex => ex.UseNewtonsoftJson());
            var req = new RestRequest("/Auth/register", Method.Post).AddJsonBody(requestAdapter);
            var res = await client.ExecuteAsync<LoginResponse>(req);

            if (res.IsSuccessful)
            {
                Snackbar!.Add("Registrado com sucesso!", Severity.Success);
                NavigationManager.NavigateTo("/");
                return;
            }

            Snackbar!.Add("Erro ao registrar, tente mais tarde!", Severity.Error);
        }
    }
}