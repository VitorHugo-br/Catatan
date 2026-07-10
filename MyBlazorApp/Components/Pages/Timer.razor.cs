
using Microsoft.AspNetCore.Components;
using MyBlazorApp.interfaces;
using MyBlazorApp.Models;

namespace MyBlazorApp.Components.Pages;

public partial class Timer(IChamadoTimerService timerService)
{
    [Parameter]
    public int ChamadoId { get; set; }

    private TimerState _state = new();

    protected override void OnInitialized()
    {
        _state = timerService.GetState(ChamadoId);
        _state.OnChange += OnChange;
    }

    private void OnChange() => InvokeAsync(StateHasChanged);

    private void TogglePause() => timerService.TogglePause(ChamadoId);

    public void Dispose() => _state.OnChange -= OnChange;
}