using Microsoft.AspNetCore.Components;
using MyBlazorApp.interfaces;
using MyBlazorApp.Services;
using MyBlazorApp.Utils;

namespace MyBlazorApp.Components.Pages;

public partial class InitialPage(NavigationManager nav, TokenProvider tokenProvider, IChamadoService service)
{

    private bool EhLogin = true;

    private string Titulo => EhLogin ? "Autenticar" : "Cadastrar";

    private void AlternarLoginCadastro() => EhLogin = !EhLogin;

    protected override async Task OnInitializedAsync()
    {
        var tavalido = await service.ValidarTokenAsync();
        if (tavalido) nav.NavigateTo("home");
    }

}