using Forum_Project1.Models;

namespace Forum_Project1.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync();
    }
}
