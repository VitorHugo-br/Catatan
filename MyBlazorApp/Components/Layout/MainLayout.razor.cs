using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor;
using MyBlazorApp.Components.Pages;
using MyBlazorApp.Utils;

namespace MyBlazorApp.Components.Layout
{
    public partial class MainLayout(
        RequestUtil requestUtil, 
        NavigationManager nav,
        ProtectedSessionStorage pss,
        IDialogService dialog
    ) : LayoutComponentBase
    {
        
        protected override async Task OnInitializedAsync()
        {
            var token = await requestUtil.GetTokenFromSessionStorage();
            if (token.Value is null) nav.NavigateTo("/");
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
    }
}