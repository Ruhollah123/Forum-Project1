using Forum_Project1.Data;
using Forum_Project1.Models;
using Forum_Project1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forum_Project1.Repositories
{
    public class PrivateMessageRepository : IPrivateMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public PrivateMessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PrivateMessage>> GetConversationHistoryAsync(string senderId, string receiverId)
        {
            return await _context.PrivateMessages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                      (m.SenderId == receiverId && m.ReceiverId == senderId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<List<PrivateMessage>> GetUserConversationsAsync(string currentUserId)
        {
            return await _context.PrivateMessages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int messageId)
        {
            var message = await _context.PrivateMessages.FindAsync(messageId);

            if (message != null)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PrivateMessage> SendMessageAsync(PrivateMessage message)
        {
            _context.PrivateMessages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }
    }
}
