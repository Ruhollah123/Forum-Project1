using Forum_Project1.Models;

namespace Forum_Project1.Services
{
    public interface ITopicService
    {
        Task<List<TopicDto>> GetAllTopicsAsync();
        Task<Topic?> GetTopicByIdAsync(int id);
        Task<Topic> CreateTopicAsync(Topic topic);
        Task<List<Topic>> GetTopicsBySubCategoryIdAsync(int subCategoryId);
    }
}
