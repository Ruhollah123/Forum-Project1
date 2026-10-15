using Forum_Project1.Data;
using Forum_Project1.Models;
using Forum_Project1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forum_Project1.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddLikeAsync(int postId, string userId)
        {
            var existingLike = await _context.Interactions
                .FirstOrDefaultAsync(i => i.PostId == postId && i.UserId == userId && i.ReactionType == "like");

            if (existingLike == null)
            {
                var interaction = new Interaction
                {
                    PostId = postId,
                    UserId = userId,
                    ReactionType = "like",
                    CreatedAt = DateTime.Now
                };

                _context.Interactions.Add(interaction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<List<Post>> GetPostsByTopicIdAsync(int topicId)
        {
            return await _context.Posts
                .Include(p => p.ApplicationUser)
                .Where(p => p.TopicId == topicId)
                .ToListAsync();
        }
    }
}