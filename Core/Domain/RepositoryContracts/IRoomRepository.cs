using EventManger.Core.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManger.Core.Domain.RepositoryContracts
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<Room> GetRoomByIDAsync(Guid id);
        Task AddRoomAsync(Room room);
        Task DeleteRoomAsync(Guid id);
        Task EditRoomAsync(Room room);
    }
}
