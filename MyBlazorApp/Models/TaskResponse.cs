using System.Net.NetworkInformation;

namespace MyBlazorApp.Models
{
    public class TaskResponse
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Status { get; set; }
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
        public DateTime? DueDate { get; set; }
        public int IssuerId { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
