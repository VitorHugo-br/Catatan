using Microsoft.AspNetCore.Components;

namespace MyBlazorApp.Components.Pages;

public partial class PageSelector : ComponentBase
{
    [Parameter] public int TotalPages { get; set; }
    private int _currentPage;
    private int _windowStart;
    private const int WindowSize = 10;
    private int WindowEnd => Math.Min(_windowStart + WindowSize, TotalPages);

    [Parameter] public EventCallback<int> OnPageChanged { get; set; }

    private async Task GoToPage(int page)
    {
        _currentPage = page;
        await OnPageChanged.InvokeAsync(_currentPage);
    }

    private void NextPage()
    {
        if (_currentPage < TotalPages - 1)
            _ = GoToPage(_currentPage + 1);

        if (_currentPage >= WindowEnd && WindowEnd < TotalPages)
            _windowStart = WindowEnd;
    }

    private void PreviousPage()
    {
        if (_currentPage > 0)
            _ = GoToPage(_currentPage - 1);

        if (_currentPage < _windowStart && _windowStart > 0)
            _windowStart = Math.Max(0, _windowStart - WindowSize);
    }
}