using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using MudBlazor;
using MyBlazorApp.interfaces;

namespace MyBlazorApp.Components.Forms;

public partial class LoginForm(IChamadoService service, IMemoryCache cache)
{
    [Parameter] public EventCallback<bool> OnRegisterClicked { get; set; }
    [Inject] private NavigationManager Nav { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private bool _success;
    private MudForm _form;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string[] _errors = [];

    private bool mostrarSenha;
    private InputType InputType => mostrarSenha ? InputType.Text : InputType.Password;
    private string IconeVisibilidadeSenha => mostrarSenha ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;

    private async Task AutenticarLoginAsync()
    {
        string msgToast;

        var token = await service.AutenticarAsync(_email, _password);
        if (token == null)
        {
            msgToast = "Erro ao autenticar. Por favor, verifique suas credenciais e tente novamente.";
            Snackbar.Add(msgToast, Severity.Error);
            return;
        }

        msgToast = "Login bem-sucedido!";
        Snackbar.Add(msgToast, Severity.Success);

        var code = Guid.NewGuid().ToString("N");
        cache.Set(code, token, TimeSpan.FromSeconds(30));

        Nav.NavigateTo($"/login-complete?code={code}", forceLoad: true);
    }

    private void AlternarVisibilidadeSenha() => mostrarSenha = !mostrarSenha;

    private async Task MudarPagina() => await OnRegisterClicked.InvokeAsync(true);


}