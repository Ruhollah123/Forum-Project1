using Forum_Project1.Data;

namespace Forum_Project1.Models
{
    public class Interaction
    {
        public int Id { get; set; }
        public string ReactionType { get; set; }
        public DateTime CreatedAt { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string UserId { get; set; }
    }
}