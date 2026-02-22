namespace MyBlazorApp.Models
{
    public class SearchNoteModel
    {
        public string? TaskID { get; set; }
        public string? UserID { get; set; }
        public string? TaskIssuerID { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? Status { get; set; }

    }
}
