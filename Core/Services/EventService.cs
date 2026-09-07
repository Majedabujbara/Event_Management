using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.Identity;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Core.ServicesContracts;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EventManger.Core.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository  _event;
        public EventService(IEventRepository Events)
        {
            _event = Events;
        }

        public async Task AddAttendeeAsync(ApplicationUser user, Event ev)
        {
            await _event.AddAttendeeAsync(user,ev);
        }

        public async Task<Event> AddEventAsync(EventsRequestDTO EventDTO)
        {
            Event Event = new Event
            {
                Name = EventDTO.Name,
                Description = EventDTO.Description,
                StartTime = EventDTO.StartTime,
                EndTime = EventDTO.EndTime,
                RoomID = EventDTO.RoomID,
                OrganizationID = EventDTO.OrganizationID,
                Status = EventDTO.Status,
                PhotoUrl = EventDTO.PhotoUrl
            };
            await _event.AddEventAsync(Event);
            return Event;
        }

        public async Task DeleteAttendeeAsync(ApplicationUser user, Event ev)
        {
            await _event.DeleteAttendeeAsync(user, ev);
        }

        public async Task EditEventAsync(Guid Id,EventsRequestDTO Event)
        {
            Event ev = await _event.GetEventAsync(Id);
            if(ev == null)
            {
                throw new Exception("Given event doens't exist");
            }
            try
            {
                ev.Name = Event.Name;
                ev.Description = Event.Description;
                ev.StartTime = Event.StartTime;
                ev.EndTime = Event.EndTime;
                ev.RoomID = Event.RoomID;
                ev.OrganizationID = Event.OrganizationID;
                ev.PhotoUrl = Event.PhotoUrl;
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException($"Id : {Id} doesn't exist");
            }catch (Exception ex)
            {
                throw new Exception("Error in updating the event");
            }
            await _event.EditEventAsync(ev);
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _event.GetAllEventsAsync();
        }

        public async Task<Event> GetEventAsync(Guid id)
        {
            return await _event.GetEventAsync(id);
        }

        public async Task RemoveEventAsync(Guid id)
        {
            await _event.RemoveEventAsync(id);
        }
    }
}
