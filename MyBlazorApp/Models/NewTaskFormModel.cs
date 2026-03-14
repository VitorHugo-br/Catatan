using System.ComponentModel.DataAnnotations;

namespace MyBlazorApp.Models;

public class NewTaskFormModel
{
    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;
    
    public int Status { get; set; } = 0;
    
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? DueDate { get; set; }
    
    public int IssuerId { get; set; }
    public int? UserId { get; set; }
}