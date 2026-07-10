using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using MyBlazorApp.interfaces;
using MyBlazorApp.Models;
using MyBlazorApp.Models.DTO;
using MyBlazorApp.Services;

namespace MyBlazorApp.Components.Pages;

public partial class ViewChamado(IChamadoService service, IChamadoTimerService timerService, IJSRuntime js) : IDisposable
{
    [Parameter]
    public int ChamadoId { get; set; }
    private TaskApiResponse? Chamado { get; set; }

    private TimerState? _timerState;

    private string IconeTimer => _timerState is { IsPaused: false } ? Icons.Material.Filled.PauseCircle : Icons.Material.Filled.PlayCircle;

    protected override async Task OnInitializedAsync()
    {
        Chamado = await service.GetChamadoPorIdAsync(ChamadoId);

        _timerState = timerService.GetState(ChamadoId);
        _timerState.OnChange += HandleTimerChange;
    }

    private void HandleTimerChange()
    {
        InvokeAsync(StateHasChanged);
    }

    public async Task Timer()
    {
        timerService.TogglePause(ChamadoId);
        await js.InvokeVoidAsync("abrirJanelaTimer", ChamadoId, $"/timer-window/{ChamadoId}");
    }

    public void Dispose()
    {
        _timerState?.OnChange -= HandleTimerChange;
    }
}