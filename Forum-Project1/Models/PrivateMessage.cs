using Forum_Project1.Data;

namespace Forum_Project1.Models
{
    public class PrivateMessage
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
        public string SenderId { get; set; }
        public ApplicationUser? Sender { get; set; }
        public string ReceiverId { get; set; }
        public ApplicationUser? Receiver { get; set; }
    }
}
