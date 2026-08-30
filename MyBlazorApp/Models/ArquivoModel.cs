using Microsoft.AspNetCore.Components.Forms;

namespace MyBlazorApp.Models;

public class ArquivoModel
{
    public IReadOnlyList<IBrowserFile>? Arquivos { get; set; } = [];
}
