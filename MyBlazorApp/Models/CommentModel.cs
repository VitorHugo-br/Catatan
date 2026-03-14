namespace MyBlazorApp.Models;

public class CommentModel
{
    public int CommentId { get; set; }
    public int TaskId { get; set; }
    public int IssuerId { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}