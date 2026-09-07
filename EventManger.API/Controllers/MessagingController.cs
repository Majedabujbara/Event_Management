using System.Threading.Tasks;
using EventManger.Core.Domain.Entites;
using EventManger.Core.ServicesContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManger.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagingController : ControllerBase
    {
        private readonly IChatService _chatService;
        public MessagingController(IChatService chatService)
        {
            _chatService = chatService;
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(string message, string email)
        {
            await _chatService.SendMessageAsync(email, message);
            return Ok();
        }
        [HttpGet]
        public async Task<IEnumerable<Message>> GetMessages(Guid guid)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                // Handle WebSocket communication here
            }
                // Return regular HTTP response
                return await _chatService.GetMessagesAsync(guid);
        }
    }
}
