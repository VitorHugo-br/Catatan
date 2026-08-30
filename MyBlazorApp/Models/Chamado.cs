
 
namespace MyBlazorApp.Models;
 
public class Chamado
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Status Status { get; set; } = Status.Pendente;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int IssuerId { get; set; }
    public int? UserId { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public enum Status
{
    Pendente,
    Desenvolvimento,
    Concluido,
    Entregue,
}