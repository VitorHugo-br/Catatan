namespace MyBlazorApp.Components.Pages;

public partial class InitialPage
{

    private bool EhLogin = true;

    private string Titulo => EhLogin ? "Autenticar" : "Cadastrar";

    private void AlternarLoginCadastro() => EhLogin = !EhLogin;

}