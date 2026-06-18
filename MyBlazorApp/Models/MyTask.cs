
 
namespace MyBlazorApp.Models;
 
public class MyTask
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Status Status { get; set; } = Status.Pending;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int IssuerId { get; set; }
    public int? UserId { get; set; }
    public DateTime? CreatedAt { get; set; }
}
 
public enum Status
{
    Pending = 0,
    InProgress = 1,
    Done = 2
}