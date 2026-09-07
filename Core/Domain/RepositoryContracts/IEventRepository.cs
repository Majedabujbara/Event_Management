using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManger.Core.Domain.RepositoryContracts
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventAsync(Guid id);
        //event word normally will cause an error 
        Task AddEventAsync(Event Event);
        Task RemoveEventAsync(Guid id);
        Task EditEventAsync(Event Event);
        Task AddAttendeeAsync(ApplicationUser user,Event ev);
        Task DeleteAttendeeAsync(ApplicationUser user, Event ev);

    }
}
