using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Core.ServicesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManger.Core.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _room;
        public RoomService(IRoomRepository room)
        {
            _room= room;
        }

        public async Task<Room> AddRoomAsync(RoomRequestDTO roomRequest)
        {
            Room room = new Room
            {
                Name = roomRequest.Name,
                Description = roomRequest.Description,
                Seats = roomRequest.Seats
            };
            await _room.AddRoomAsync(room);
            return room;
        }

        public async Task DeleteRoomAsync(Guid id)
        {
            await _room.DeleteRoomAsync(id);
        }

        public async Task EditRoomAsync(Guid id, RoomRequestDTO roomRequestDTO)
        {
            Room room = await GetRoomByIDAsync(id);
            if(room ==null)
            {
                throw new Exception("No room with such an ID exists to be edited");
            }
            room.Name = roomRequestDTO.Name;
            room.Description = roomRequestDTO.Description;
            room.Seats = roomRequestDTO.Seats;
            await _room.EditRoomAsync(room);
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _room.GetAllRoomsAsync();
        }

        public async Task<Room> GetRoomByIDAsync(Guid id)
        {
            return await _room.GetRoomByIDAsync(id);
        }
    }
}
