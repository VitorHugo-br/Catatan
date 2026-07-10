using MyBlazorApp.interfaces;
using MyBlazorApp.Models.DTO;
using MyBlazorApp.Models;

namespace MyBlazorApp.Components.Pages;

public partial class Notes(IChamadoService chamadoService)
{

    public List<TaskApiResponse>? Data { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Data = await chamadoService.GetDataForTasksPage();
    }

    private Func<TaskApiResponse, string> _cellStyleFunc => x =>
    {
        string style = "";
        switch (x.Status)
        {
            case Status.Pendente:
                style = "background-color: #FF746C; color: black;";
                break;
            case Status.Desenvolvimento:
                style = "background-color: #FFEE8C; color: black;";
                break;
            case Status.Entregue:
                style = "background-color: #80EF80; color: black;";
                break;
        }

        return style;
    };

}