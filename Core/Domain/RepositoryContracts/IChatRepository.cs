using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManger.Core.Domain.Entites;

namespace EventManger.Core.Domain.RepositoryContracts
{
    public interface IChatRepository
    {
        public Task SendMessageAsync(string email, string messageContent);
        public Task<IEnumerable<Message>> GetMessagesAsync(Guid userId);
    }
}
