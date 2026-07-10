namespace MyBlazorApp.Models.DTO;

public class TaskDto()
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Status Status { get; set; } = Status.Pendente;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int? UserId { get; set; }
}

public class FilterTasksDto
{
    public int? TaskId { get; set; }
    public int? UserId { get; set; }
    public int? IssuerId { get; set; }
    public Status? Status { get; set; }
    public DateTime? CreationDate { get; set; }
    public DateTime? DueDate { get; set; }
}

public record LoginDto(string email, string password);


public record LoginResponse(string token);