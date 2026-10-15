using Forum_Project1.Models;
using Microsoft.AspNetCore.Identity;

namespace Forum_Project1.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsBlocked { get; set; }
        public List<Interaction> Interactions { get; set; }
        public List<Post> Posts { get; set; }
        public List<Report> Reports { get; set; }
        public List<Topic> Topics { get; set; }
        public List<PrivateMessage>? SentMessages { get; set; }
        public List<PrivateMessage>? ReceivedMessages { get; set; }
    }

}
