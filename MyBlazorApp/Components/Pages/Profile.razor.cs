using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using MyBlazorApp.Models;

namespace MyBlazorApp.Components.Pages;

public partial class Profile : ComponentBase
{
    private ProfileInfoModel piModel { get; set; } = new();

    [Inject] private IDialogService DialogService { get; set; }

    private void EditarPerfil(EditContext obj)
    {
        throw new NotImplementedException();
    }

    private Task<IDialogReference> ExcluirConta()
    {
        var options = new DialogOptions { CloseButton = true, Position = DialogPosition.TopCenter};
        return DialogService.ShowAsync<ExcluirContaDialog>("Excluir Perfil", options);
    }
}