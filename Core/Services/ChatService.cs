using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Core.ServicesContracts;

namespace EventManger.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(Guid userId)
        {
            return await _chatRepository.GetMessagesAsync(userId);
        }

        public async Task SendMessageAsync(string email, string messageContent)
        {
            await _chatRepository.SendMessageAsync(email, messageContent);
        }
    }
}
