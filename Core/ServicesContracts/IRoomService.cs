using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManger.Core.ServicesContracts
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<Room> GetRoomByIDAsync(Guid id);
        Task<Room> AddRoomAsync(RoomRequestDTO room);
        Task DeleteRoomAsync(Guid id);
        Task EditRoomAsync(Guid Id,RoomRequestDTO room);
    }
}
