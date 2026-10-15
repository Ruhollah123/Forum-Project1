using Forum_Project1.Data;

namespace Forum_Project1.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ReportedByUserId { get; set; }
        public bool IsResolved { get; internal set; }
    }
}
