namespace MyBlazorApp.Models;

public class TimerState
{
    public TimeSpan Elapsed { get; set; } = TimeSpan.Zero;
    public bool IsPaused { get; set; } = true;

    public event Action? OnChange;
    public void NotifyChange() => OnChange?.Invoke();
}
