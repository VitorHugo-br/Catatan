using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor;
using MyBlazorApp.Components.Pages;
using MyBlazorApp.Utils;

namespace MyBlazorApp.Components.Layout;

public partial class MainLayout(
    RequestUtil requestUtil, 
    NavigationManager nav,
    ProtectedSessionStorage pss,
    IDialogService dialog,
    LocalStorageService localStorageService
) : LayoutComponentBase
{

    private bool _open;

    private bool _modoEscuro;
    private string ModoEscuroIcone => _modoEscuro ? Icons.Material.Filled.LightMode : Icons.Material.Filled.DarkMode;

    private string NumeroChamado = string.Empty;

    //protected override async Task OnInitializedAsync()
    //{
    //    var token = await requestUtil.GetTokenFromSessionStorage();
    //    if (token.Value is null) nav.NavigateTo("/");
    //}

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            string? darkMode = await localStorageService.GetItemAsync("darkMode");
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

    private async void Sair()
    {
        try
        {
            await pss.DeleteAsync("authToken");
            nav.NavigateTo("/");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void BuscarChamado()
    {

    }

    private async Task SaveTheme() => await localStorageService.SetItemAsync("darkMode", _modoEscuro.ToString());

    private void NavegarPara(string pagina) => nav.NavigateTo($"{pagina}");
}