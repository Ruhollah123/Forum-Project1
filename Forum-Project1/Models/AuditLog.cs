using Forum_Project1.Data;

namespace Forum_Project1.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime TimeStamp { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int UserId { get; set; }
        public Post? Post { get; set; }
        public int? PostId { get; set; }
    }
}