using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.RegularExpressions;

namespace MyBlazorApp.Components.Forms;

public partial class RegisterForm
{
    [Parameter] public EventCallback<bool> OnLoginClicked { get; set; }

    private bool _success;
    private MudForm _form;
    private string _name;
    private string _email;
    private string _password;
    private string[] _errors = [];

    private static IEnumerable<string> PasswordStrength(string pw)
    {
        if (string.IsNullOrWhiteSpace(pw))
        {
            yield return "Senha é obrigatório!";
            yield break;
        }
        if (pw.Length < 8) yield return "Senha deve ter pelo menos 8 caracteres.";

        if (!Regex.IsMatch(pw, @"[A-Z]")) yield return "Senha deve conter pelo menos 1 letra maiúscula.";

        if (!Regex.IsMatch(pw, @"[a-z]")) yield return "Senha deve conter pelo menos 1 letra minúscula.";

        if (!Regex.IsMatch(pw, @"[0-9]")) yield return "Senha deve conter pelo menos 1 número.";
    }

    private string? PasswordMatch(string arg) => _password != arg ? "Passwords don't match" : null;

    private async Task MudarPagina() => await OnLoginClicked.InvokeAsync(true);

}