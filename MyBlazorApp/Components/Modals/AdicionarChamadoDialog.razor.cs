using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyBlazorApp.Dto;
using MyBlazorApp.interfaces;
using MyBlazorApp.Models;
using MyBlazorApp.Models.DTO;
using MyBlazorApp.Validators;

namespace MyBlazorApp.Components.Modals;

public partial class AdicionarChamadoDialog(IChamadoService service) : ComponentBase
{
    [Inject] private ISnackbar Snackbar { get; set; }

    private MudForm _form;
    private AdicionarChamadoFormModel _formModel = new();
    private ChamadoValidator _validator = new();
    private IEnumerable<UsuarioDto>? Usuarios { get; set; } = [];

    private const string DefaultDragClass = "relative d-flex flex-column p-r gap-2 rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
    private string _dragClass = DefaultDragClass;
    private bool _dragging = false;

    protected override async Task OnInitializedAsync()
    {
        Usuarios = await service.ObterUsuariosAsync();
    }

    private void SetDragClass()
    {
        _dragClass = $"{DefaultDragClass} mud-border-success";
        _dragging = true;
    }

    private void ClearDragClass()
    {
        if (!_dragging)
        {
            _dragClass = DefaultDragClass;
        }
    }

    private async Task Submit()
    {
        await _form.ValidateAsync();

        if (!_form.IsValid)
        {
            return;
        }

        var task = new TaskDto
        {
            Title = _formModel.Title,
            Description = _formModel.Description,
            DueDate = _formModel.DueDate,
            StartDate = DateTime.Now,
            Status = (Status)_formModel.Status,
            UserId = _formModel.UserId,
        };

        var (numeroChamado, criado) = await service.CriarChamadoAsync(task);

        if (!criado)
        {
            Snackbar.Add("Algo deu errado", Severity.Error);
            return;
        }

        if (_formModel.Arquivos.Any())
        {
            var sucesso = await service.UploadArquivosAsync(numeroChamado, _formModel.Arquivos);

            if (!sucesso)
            {
                Snackbar.Add("Chamado criado, mas falha ao enviar arquivos.", Severity.Warning);
                return;
            }
        }

        Snackbar.Add("Tarefa criada com sucesso", Severity.Success);
    }
}