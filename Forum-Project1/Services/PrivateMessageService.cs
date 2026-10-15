using Forum_Project1.Models;
using Forum_Project1.Repositories.Interfaces;

namespace Forum_Project1.Services
{
    public class PrivateMessageService : IPrivateMessageService
    {
        private readonly IPrivateMessageRepository _privateMessageRepository;

        public PrivateMessageService(IPrivateMessageRepository privateMessageRepository)
        {
            _privateMessageRepository = privateMessageRepository;
        }

        public async Task<List<PrivateMessage>> GetConversationHistoryAsync(string currentUserId, string otherUserId)
        {
            return await _privateMessageRepository.GetConversationHistoryAsync(currentUserId, otherUserId);
        }

        public async Task<List<PrivateMessage>> GetUserConversationsAsync(string currentUserId)
        {
            return await _privateMessageRepository.GetUserConversationsAsync(currentUserId);
        }

        public async Task MarkAsReadAsync(int messageId)
        {
            await _privateMessageRepository.MarkAsReadAsync(messageId);
        }

        public async Task<PrivateMessage> SendMessageAsync(string senderId, string receiverId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Meddelandet kan inte vara tomt");
            }

            if (senderId == receiverId)
            {
                throw new InvalidOperationException("Du kan inte skicka meddelanden till dig själv");
            }

            var message = new PrivateMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content.Trim(),
                SentAt = DateTime.Now,
                IsRead = false
            };

            return await _privateMessageRepository.SendMessageAsync(message);
        }

    }
}
