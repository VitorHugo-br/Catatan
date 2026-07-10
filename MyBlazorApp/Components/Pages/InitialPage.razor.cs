using Microsoft.AspNetCore.Components;
using MyBlazorApp.Services;
using MyBlazorApp.Utils;

namespace MyBlazorApp.Components.Pages;

public partial class InitialPage(NavigationManager nav, TokenProvider tokenProvider)
{

    private bool EhLogin = true;

    private string Titulo => EhLogin ? "Autenticar" : "Cadastrar";

    private void AlternarLoginCadastro() => EhLogin = !EhLogin;

    protected override async Task OnInitializedAsync()
    {
        if (tokenProvider.IsAuthenticated) nav.NavigateTo("/home/");
    }

}