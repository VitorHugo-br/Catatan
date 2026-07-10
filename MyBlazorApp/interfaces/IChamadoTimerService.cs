using MyBlazorApp.Models;

namespace MyBlazorApp.interfaces;

public interface IChamadoTimerService
{
    TimerState GetState(int chamadoId);
    void TogglePause(int chamadoId);
}
