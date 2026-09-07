using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManger.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public RoomRepository(ApplicationDbContext context)
        {
            _context= context;
        }
        public async Task AddRoomAsync(Room room)
        {
            if(await _context.rooms.SingleOrDefaultAsync(x=>x.RoomID==room.RoomID)!= null)
            {
                throw new Exception("Room already exists");
            }
            var validationContext = new ValidationContext(room);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(room, validationContext, validationResults, true);
            if(isValid)
            {
                await _context.rooms.AddAsync(room);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Error in the rooms entity");
            }
        }

        public async Task DeleteRoomAsync(Guid id)
        {
            if(await _context.rooms.SingleOrDefaultAsync(x => x.RoomID == id) != null)
            {
                var en = await _context.rooms.SingleOrDefaultAsync(x => x.RoomID == id);
                try
                {
                    _context.rooms.Remove(en);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("Couldn't find an entity with the given ID");
            }
        }

        public async Task EditRoomAsync(Room room)
        {
            Room? toBeEdited = await _context.rooms.SingleOrDefaultAsync(x=>x.RoomID == room.RoomID);
            if(toBeEdited == null)
            {
                throw new Exception("No room with such an ID exists to be edited");
            }
            try
            {
                _context.Entry(room).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _context.rooms.ToListAsync();
        }

        public async Task<Room> GetRoomByIDAsync(Guid id)
        {
            if(await _context.rooms.SingleOrDefaultAsync(x => x.RoomID == id)==null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return await _context.rooms.SingleOrDefaultAsync(x => x.RoomID == id);
        }

    }
}
