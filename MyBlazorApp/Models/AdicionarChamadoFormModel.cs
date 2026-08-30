
using Microsoft.AspNetCore.Components.Forms;

namespace MyBlazorApp.Models;

public class AdicionarChamadoFormModel
{
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public int Status { get; set; } = 0;
    
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? DueDate { get; set; }
    
    public int IssuerId { get; set; }
    public int UserId { get; set; }

    public IReadOnlyList<IBrowserFile> Arquivos { get; set; } = [];
}