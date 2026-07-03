using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor;

namespace MyBlazorApp.Components.Forms;

public partial class LoginForm
{
    [Parameter] public EventCallback<bool> OnRegisterClicked { get; set; }

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ProtectedSessionStorage? Pss { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private bool _success;
    private MudForm _form;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string[] _errors = [];

    private bool mostrarSenha;
    private InputType InputType => mostrarSenha ? InputType.Text : InputType.Password;
    private string IconeVisibilidadeSenha => mostrarSenha ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;

    private Task<string> ValidarLoginAsync()
    {
        if (string.IsNullOrWhiteSpace(_email) || string.IsNullOrWhiteSpace(_password))
        {
            _errors = ["Email and password are required."];
            return Task.FromResult(string.Empty);
        }
        
        

        _errors = Array.Empty<string>();
        return Task.FromResult(string.Empty);
    }

    private void AlternarVisibilidadeSenha() => mostrarSenha = !mostrarSenha;

    private async Task MudarPagina() => await OnRegisterClicked.InvokeAsync(true);
    

}