using MyBlazorApp.Dto;
using MyBlazorApp.interfaces;

namespace MyBlazorApp.Components.Pages;

public partial class GruposPage(IChamadoService service)
{
    private IEnumerable<Grupo>? Grupos = [];

    protected override async Task OnInitializedAsync()
    {
        Grupos = await service.ObterGruposAsync();
    }
}