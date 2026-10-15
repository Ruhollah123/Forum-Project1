using Forum_Project1.Models;

namespace Forum_Project1.Services
{
    public interface IPrivateMessageService
    {
        Task<PrivateMessage> SendMessageAsync(string senderId, string receiverId, string content);
        Task<List<PrivateMessage>> GetConversationHistoryAsync(string currentUserId, string otherUserId);
        Task<List<PrivateMessage>> GetUserConversationsAsync(string currentUserId);
        Task MarkAsReadAsync(int messageId);
    }
}
