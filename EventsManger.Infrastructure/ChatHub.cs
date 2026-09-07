using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManger.Infrastructure.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace EventManger.Infrastructure
{
    public sealed class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            // Handle when a client connects
            await Clients.All.SendAsync("ReceiveMessage",$"{Context.ConnectionId} has joined");
        }
        public async Task SendMessageAsync(string email, string messageContent)
        {
            var chatService = Context.GetHttpContext().RequestServices.GetService<ChatRepository>();
            await chatService.SendMessageAsync(email, messageContent);
        }
    }
}
