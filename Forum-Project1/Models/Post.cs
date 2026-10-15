using Forum_Project1.Data;

namespace Forum_Project1.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ImageUrl { get; set; }
        public Topic? Topic { get; set; }
        public int TopicId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public string UserId { get; set; }
        public List<Interaction> Interactions { get; set; }
    }
}
