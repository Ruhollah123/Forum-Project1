using Forum_Project1.Models;

namespace Forum_Project1.Repositories.Interfaces
{
    public interface IPrivateMessageRepository
    {
        Task<PrivateMessage> SendMessageAsync(PrivateMessage message);
        Task<List<PrivateMessage>> GetConversationHistoryAsync(string senderId, string receiverId);
        Task<List<PrivateMessage>> GetUserConversationsAsync(string currentUserId);
        Task MarkAsReadAsync(int messageId);
    }
}
