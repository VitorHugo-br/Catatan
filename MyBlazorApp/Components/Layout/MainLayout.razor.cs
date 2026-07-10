using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor;
using MyBlazorApp.Components.Pages;
using MyBlazorApp.interfaces;
using MyBlazorApp.Services;
using MyBlazorApp.Utils;

namespace MyBlazorApp.Components.Layout;

public partial class MainLayout(
    NavigationManager nav,
    IDialogService dialog,
    ILocalStorageService localStorage,
    TokenProvider tokenProvider
) : LayoutComponentBase
{

    private bool _open;

    private bool _modoEscuro;
    private string ModoEscuroIcone => _modoEscuro ? Icons.Material.Filled.LightMode : Icons.Material.Filled.DarkMode;

    private string NumeroChamado = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        if(!tokenProvider.IsAuthenticated) nav.NavigateTo("/", forceLoad: true);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            string? darkMode = await localStorage.GetItemAsync("darkMode");
            if(bool.TryParse(darkMode, out bool result))
            {
                _modoEscuro = result;
            }
            else
            {
                _modoEscuro = false;
            }
            StateHasChanged();
        }
    }
    
    private Task<IDialogReference> AdicionarTarefa()
    {
       var options = new DialogOptions
       {
           CloseButton = true, 
           Position = DialogPosition.Center,
           FullWidth = true,
           
       };
       return dialog.ShowAsync<AdicionarTarefaDialog>("Adicionar tarefa.",options);
    }

    private async void Sair() => nav.NavigateTo("/api/logout", forceLoad: true);

    private void BuscarChamado()
    {
        var numero = NumeroChamado;
        NumeroChamado = string.Empty;
        nav.NavigateTo($"/Chamados/{numero}");
    }

    private async Task SaveTheme() => await localStorage.SetItemAsync("darkMode", _modoEscuro.ToString());

    private void NavegarPara(string pagina) => nav.NavigateTo($"{pagina}");
}