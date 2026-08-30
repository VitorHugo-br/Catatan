using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyBlazorApp.Dto;
using MyBlazorApp.interfaces;

namespace MyBlazorApp.Components.Modals;

public partial class AdicionarGrupoDialog(IChamadoService chamadoService)
{

    private MudForm _form;
    private string _nome = string.Empty;
    private string[] _errors = [];
    private bool _success;

    [Inject] private ISnackbar Snackbar { get; set; }

    private IEnumerable<UsuarioDto>? _usuarios = [];

    private IReadOnlyCollection<UsuarioDto> _usuariosSelecionados = [];

    protected override async Task OnInitializedAsync()
    {
        _usuarios = await chamadoService.ObterUsuariosAsync();
    }

    private async Task CriarGrupoAsync()
    {
        try
        {
            var (id, usuariosAdicionados) = await chamadoService.CriarGrupoAsync(_nome, _usuariosSelecionados);
            if (id > 0 && usuariosAdicionados)
            {
                Snackbar.Add($"Usuarios adicionados ao grupo {id}", severity: Severity.Success);
                return;
            }

            if (id > 0 && !usuariosAdicionados)
            {
                Snackbar.Add($"Grupo {id} criado", severity: Severity.Success);
                return;
            }
        }
        catch (Exception ex)
        {

            Snackbar.Add("Houve um problema na criacao do grupo, tente mais tarde.", severity: Severity.Error);
            return;
        }

    }

}