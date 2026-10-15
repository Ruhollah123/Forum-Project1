using Forum_Project1.Models;

namespace Forum_Project1.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetPostsByTopicIdAsync(int topicId);
        Task<Post> CreatePostAsync(Post post);
        Task AddLikeAsync(int postId, string userId);
    }
}
