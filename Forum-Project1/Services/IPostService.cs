using Forum_Project1.Models;

namespace Forum_Project1.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsByTopicIdAsync(int topicId);
        Task<Post?> CreatePostAsync(Post post);
        Task AddLikeAsync(int postId, string userId);
    }
}
