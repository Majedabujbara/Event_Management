using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.Identity;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Infrastructure.DBContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EventManger.Infrastructure.Repositories
{
    public class ChatRepository:IChatRepository
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public ChatRepository(IHubContext<ChatHub> hubContext, ApplicationDbContext dbContext,UserManager<ApplicationUser> userManager)
        {
            _hubContext = hubContext;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(Guid userId)
        {
            return await _dbContext.Messages
                .Where(m => m.UserId == userId)
                .ToListAsync();
        }

        public async Task SendMessageAsync(string email, string messageContent)
        {
            var user = await _userManager.FindByNameAsync(email);
            if (user == null)
            {
                throw new ArgumentException($"User '{email}' not found.");
                //user = new  { Name = userName };
                //_dbContext.Users.Add(user);
                //await _dbContext.SaveChangesAsync();
            }

            var message = new Message
            {
                Content = messageContent,
                Timestamp = DateTime.Now,
                UserId = user.Id
            };

            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
