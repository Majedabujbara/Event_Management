using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.Identity;
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
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;
        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAttendeeAsync(ApplicationUser user,Event ev)
        {
            var validationContext = new ValidationContext(user);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(user, validationContext, validationResults, true);

            if (!isValid)
            {
                throw new Exception("Given User Has An Error");
            }
            ev.Attendees.Add(user);
            _context.Events.Update(ev);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAttendeeAsync(ApplicationUser user, Event ev)
        {
            var validationContext = new ValidationContext(user);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(user, validationContext, validationResults, true);

            if (!isValid)
            {
                throw new Exception("Given User Has An Error");
            }
            ev.Attendees.Remove(user);
            _context.Events.Update(ev);
            await _context.SaveChangesAsync();
        }


        //should be able to add an event even if there collisions but only if the collision is with a cancelled event
        public async Task AddEventAsync(Event newEvent)
        {
            // Check if the room already has an event with conflicting timings
            bool hasConflict = await _context.Events
                .AnyAsync(e => e.RoomID == newEvent.RoomID &&
                               e.EventID != newEvent.EventID && // Exclude the current event (if updating)
                               e.StartTime < newEvent.EndTime &&
                               e.EndTime > newEvent.StartTime &&
                               e.OrganizationID == newEvent.OrganizationID);

            if (await _context.Events.SingleOrDefaultAsync(x => x.EventID == newEvent.EventID)!=null)
            {
                throw new Exception("Event ID Already Exists");
            }
            if (hasConflict || newEvent.StartTime == newEvent.EndTime || newEvent.StartTime > newEvent.EndTime || newEvent.StartTime < DateTime.Now)
            {
                throw new InvalidOperationException("Either the room is booked or the start and end times are incorrect");
            }

            // Validate the event object
            var validationContext = new ValidationContext(newEvent);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(newEvent, validationContext, validationResults, true);

            if (!isValid)
            {
                throw new Exception("Given Event Has An Error");
            }

            // Add the event to the database
            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();
        }

        public async Task EditEventAsync(Event updatedEvent)
        {
            var existingEvent = await _context.Events.FindAsync(updatedEvent.EventID);
            if (existingEvent == null)
            {
                throw new KeyNotFoundException("No event with the specified ID exists.");
            }

            _context.Entry(existingEvent).CurrentValues.SetValues(updatedEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events.Include(e=>e.Attendees).ToListAsync();
        }

        public async Task<Event> GetEventAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("Given GUID is Empty");
            }
            var entity = await _context.Events.Include(e=>e.Attendees).SingleOrDefaultAsync(x=>x.EventID==id);
            if(entity == null)
            {
                throw new KeyNotFoundException("This Event Doesn't Exits In The Database");
            }
            return entity;
        }

        public async Task RemoveEventAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("Given GUID is Empty");
            }
            var entity = await _context.Events.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException("This Event Doesn't Exits In The Database");
            }
            _context.Events.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
