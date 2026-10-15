using Forum_Project1.Data;
using Forum_Project1.Models;

namespace Forum_Project1.Services
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(int userId);
    }
}
