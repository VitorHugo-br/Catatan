using System.Net.NetworkInformation;

namespace MyBlazorApp.Models
{
    public class TaskResponse
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DueDate { get; set; }
        public string RequestedBy { get; set; }
        public int UserId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
