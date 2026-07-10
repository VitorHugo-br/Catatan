namespace MyBlazorApp.Models.DTO;

public class TaskApiResponse
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Status Status { get; set; }
    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;
    public DateTime? DueDate { get; set; }
    public int IssuerId { get; set; }
    public User? Issuer { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public string Role { get; set; }
}
