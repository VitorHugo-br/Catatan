using MyBlazorApp.interfaces;
using MyBlazorApp.Models;
using System.Collections.Concurrent;

namespace MyBlazorApp.Services;

public class ChamadoTimerService : IChamadoTimerService, IDisposable
{
    private readonly ConcurrentDictionary<int, TimerState> _timers = new();
    private readonly PeriodicTimer _ticker = new(TimeSpan.FromSeconds(1));
    private readonly CancellationTokenSource _cts = new();

    public ChamadoTimerService()
    {
        _ = RunAsync();
    }

    public TimerState GetState(int chamadoId)
        => _timers.GetOrAdd(chamadoId, _ => new TimerState());

    public void TogglePause(int chamadoId)
    {
        var state = GetState(chamadoId);
        state.IsPaused = !state.IsPaused;
        state.NotifyChange();
    }

    private async Task RunAsync()
    {
        try
        {
            while (await _ticker.WaitForNextTickAsync(_cts.Token))
            {
                foreach (var (_, state) in _timers)
                {
                    if (!state.IsPaused)
                    {
                        state.Elapsed = state.Elapsed.Add(TimeSpan.FromSeconds(1));
                        state.NotifyChange();
                    }
                }
            }
        }
        catch (OperationCanceledException) { }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _ticker.Dispose();
    }
}
