using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Core.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManger.API.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _room;
        public RoomsController(IRoomService room)
        {
            _room = room;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            return await _room.GetAllRoomsAsync();
        }
        [HttpGet("{ID}")]
        [AllowAnonymous]
        public async Task<Room> GetRoom(Guid ID)
        {
            return await _room.GetRoomByIDAsync(ID);
        }
        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteRoom(Guid ID)
        {
            await _room.DeleteRoomAsync(ID);
            return Ok($"Deleted Room:{ID}");
        }
        //check it 
        [HttpPatch]
        public async Task<IActionResult> EditRoom(Guid Id , RoomRequestDTO room)
        {
            await _room.EditRoomAsync(Id,room);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AddRoom(RoomRequestDTO roomRequest)
        {
            Room room=await _room.AddRoomAsync(roomRequest);
            return Ok(room);
        }
    }
}
