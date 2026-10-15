using Forum_Project1.Data;
using Forum_Project1.Models;
using Forum_Project1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forum_Project1.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly ApplicationDbContext _context;

        public TopicRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TopicDto>> GetAllTopicsAsync()
        {
            return await _context.Topics
        .OrderByDescending(t => t.IsPinned)
        .ThenByDescending(t => t.CreatedAt)
        .Select(t => new TopicDto
        {
            Id = t.Id,
            Title = t.Title,
            CreatedAt = t.CreatedAt,
            AuthorName = t.ApplicationUser != null
                ? (!string.IsNullOrWhiteSpace(t.ApplicationUser.Name)
                    ? t.ApplicationUser.Name
                    : (!string.IsNullOrWhiteSpace(t.ApplicationUser.UserName)
                        ? t.ApplicationUser.UserName
                        : "Unknown"))
                : "Unknown"
        })
        .ToListAsync();
        }

        public async Task<Topic> CreateTopicAsync(Topic topic)
        {
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();

            return await _context.Topics
                .Include(t => t.ApplicationUser)
                .FirstOrDefaultAsync(t => t.Id == topic.Id) ?? topic;
        }


        public async Task<Topic?> GetTopicByIdAsync(int id)
        {
            return await _context.Topics
                .Include(t => t.ApplicationUser)
                .Include(t => t.Posts)
                    .ThenInclude(p => p.ApplicationUser)
                .Include(t => t.Posts)
                .ThenInclude(p => p.Interactions)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Topic>> GetTopicsBySubCategoryIdAsync(int subCategoryId)
        {
            return await _context.Topics
                .Where(t => t.SubCategoryId == subCategoryId)
                .Include(t => t.ApplicationUser)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}