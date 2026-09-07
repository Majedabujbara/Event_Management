using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.Identity;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Core.Services;
using EventManger.Core.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManger.API.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeesController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IPersonService _personService;

        public AttendeesController(IEventService eventService, IPersonService personService)
        {
            _eventService = eventService;
            _personService = personService;
        }

        [AllowAnonymous]
        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEventAttendees(Guid eventId)
        {
            if(eventId == Guid.Empty)
            {
                return BadRequest("Invalid Event ID");
            }

            var events = await _eventService.GetEventAsync(eventId);
            if(events == null)
            {
                return NotFound("Event not found");
            }

            return Ok(events.Attendees);
        }
        [HttpPost("{eventId}")]
        [AllowAnonymous]
        public async Task<IActionResult> PostAddAttendee(Guid userId, Guid eventId)
        {
            if (userId == Guid.Empty || eventId == Guid.Empty)
            {
                return Problem("user has the wrong details");
            }

            var eve = await _eventService.GetEventAsync(eventId);
            if (eve == null)
            {
                return Problem("Event not found");
            }

            var user = await _personService.GetPersonAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            if(eve.Attendees.Any(x => x.Id == userId))
            {
                return Conflict("User alredy exists in the event");
            }

            await _eventService.AddAttendeeAsync(user, eve);
            return Ok("Attendee added successfully");
        }

        [HttpDelete("{eventId}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteAttendee(Guid userId, Guid eventId)
        {
            if(userId == Guid.Empty || eventId == Guid.Empty)
            {
                return BadRequest("Invalid user or event ID");
            }   
            
            var eve = await _eventService.GetEventAsync(eventId);
            if(eve == null)
            {
                return NotFound("Event not found");
            }

            var user = await _personService.GetPersonAsync(userId);
            if(user == null)
            {
                return NotFound("User not found");
            }

            if(!eve.Attendees.Any(x => x.Id == userId))
            {
                return NotFound("User doesn't exist in the event");
            }

            await _eventService.DeleteAttendeeAsync(user, eve);
            return Ok("Attendee removed successfully");
        }
    }
}
