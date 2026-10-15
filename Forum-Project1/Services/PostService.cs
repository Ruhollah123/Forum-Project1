using Forum_Project1.Models;
using Forum_Project1.Repositories.Interfaces;

namespace Forum_Project1.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<Post?> CreatePostAsync(Post post)
        {
            return await _postRepository.CreatePostAsync(post);
        }

        public async Task<List<Post>> GetPostsByTopicIdAsync(int topicId)
        {
            return await _postRepository.GetPostsByTopicIdAsync(topicId);
        }

        public async Task AddLikeAsync(int postId, string userId)
        {
            await _postRepository.AddLikeAsync(postId, userId);
        }
    }
}
