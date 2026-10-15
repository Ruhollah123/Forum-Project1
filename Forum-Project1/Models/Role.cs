using Forum_Project1.Data;

namespace Forum_Project1.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public List<ApplicationUser> Users { get; set; } = new();
    }
}
