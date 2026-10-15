using Forum_Project1.Models;
using Forum_Project1.Repositories.Interfaces;

namespace Forum_Project1.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<Topic> CreateTopicAsync(Topic topic)
        {
            return await _topicRepository.CreateTopicAsync(topic);
        }

        public async Task<List<TopicDto>> GetAllTopicsAsync()
        {
            return await _topicRepository.GetAllTopicsAsync();
        }

        public async Task<Topic?> GetTopicByIdAsync(int id)
        {
            return await _topicRepository
                .GetTopicByIdAsync(id);
        }

        public async Task<List<Topic>> GetTopicsBySubCategoryIdAsync(int subCategoryId)
        {
            return await _topicRepository.GetTopicsBySubCategoryIdAsync(subCategoryId);
        }
    }
}
