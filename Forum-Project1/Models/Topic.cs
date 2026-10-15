using Forum_Project1.Data;

namespace Forum_Project1.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPinned { get; set; }
        public bool IsLocked { get; set; }
        public SubCategory? SubCategory { get; set; }
        public int SubCategoryId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public string UserId { get; set; }
        public List<Post>? Posts { get; set; }
    }
}
