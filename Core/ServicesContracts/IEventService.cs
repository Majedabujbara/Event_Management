using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManger.Core.ServicesContracts
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventAsync(Guid id);
        Task<Event> AddEventAsync(EventsRequestDTO Event);
        Task RemoveEventAsync(Guid id);
        Task EditEventAsync(Guid Id, EventsRequestDTO Event);
        Task AddAttendeeAsync(ApplicationUser user, Event ev);
        Task DeleteAttendeeAsync(ApplicationUser user, Event ev);
    }
}
