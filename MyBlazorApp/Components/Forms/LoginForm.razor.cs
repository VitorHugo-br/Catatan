using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyBlazorApp.Models;

namespace MyBlazorApp.Components.Forms;

public partial class LoginForm
{
    private bool _success;
    private MudForm _form;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string[] _errors = [];

    private bool mostrarSenha;
    private InputType _inputType => mostrarSenha ? InputType.Text : InputType.Password;
    private string _iconeVisibilidadeSenha => mostrarSenha ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;

    private Task<string> ValidarLoginAsync()
    {
        if (string.IsNullOrWhiteSpace(_email) || string.IsNullOrWhiteSpace(_password))
        {
            _errors = new[] { "Email and password are required." };
            return Task.FromResult(string.Empty);
        }
        // Here you can add more complex validation logic if needed
        _errors = Array.Empty<string>();
        return Task.FromResult(string.Empty);
    }

    private void AlternarVisibilidadeSenha() => mostrarSenha = !mostrarSenha;

}