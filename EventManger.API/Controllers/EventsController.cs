using EventManger.Core.Domain.DTO;
using EventManger.Core.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManger.API.Controllers
{
    [Authorize(Roles = "Admin",AuthenticationSchemes ="Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IWebHostEnvironment _env;
        public EventsController(IEventService eventService, IWebHostEnvironment env)
        {
            _eventService= eventService;
            _env = env;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllEvents()
        {
            try
            {
                var events = await _eventService.GetAllEventsAsync();
                return Ok(events);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{EventId}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetEvent(Guid EventId)
        {
            if(EventId == Guid.Empty)
            {
                return BadRequest("Invalid event ID");
            }

            try
            {
                var result = await _eventService.GetEventAsync(EventId);
                var resultDTO = new EventsRequestDTO()
                {
                    Name = result.Name,
                    Description = result.Description,
                    StartTime = result.StartTime,
                    EndTime = result.EndTime,
                    RoomID = result.RoomID,
                    OrganizationID = result.OrganizationID,
                    Attendees = result.Attendees,
                    Status = result.Status,
                    PhotoUrl = result.PhotoUrl,
                    Views = ++result.Views
                };

                // Update the event views in the database
                await _eventService.EditEventAsync(EventId, resultDTO);

                if (result == null)
                {
                    return NotFound("Event not found");
                }
                return Ok(result);  
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddEvent([FromForm] EventsRequestDTO ev)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (ev.EventPhoto != null)
            {
                var logosFolder = Path.Combine(_env.WebRootPath, "events-photos");
                if (!Directory.Exists(logosFolder)) Directory.CreateDirectory(logosFolder);

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(ev.EventPhoto.FileName)}";
                var filePath = Path.Combine(logosFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ev.EventPhoto.CopyToAsync(stream);
                }

                ev.PhotoUrl = $"/events-photos/{uniqueFileName}";
            }

            try
            {
                var createdEvent = await _eventService.AddEventAsync(ev);
                return CreatedAtAction(nameof(GetEvent), new { EventId = createdEvent.EventID}, createdEvent);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        public async Task<ActionResult> EditEvent(Guid Id, [FromForm] EventsRequestDTO ev)
        {
            if(!ModelState.IsValid)
            {
            return BadRequest(ModelState);
            }

            if (ev.EventPhoto != null)
            {
                var logosFolder = Path.Combine(_env.WebRootPath, "events-photos");
                if (!Directory.Exists(logosFolder)) Directory.CreateDirectory(logosFolder);

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(ev.EventPhoto.FileName)}";
                var filePath = Path.Combine(logosFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ev.EventPhoto.CopyToAsync(stream);
                }

                ev.PhotoUrl = $"/events-photos/{uniqueFileName}";
            }

            try
            {
                await _eventService.EditEventAsync(Id,ev);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{EventId}")]
        public async Task<IActionResult> DeleteEvent(Guid EventId)
        {
            if (EventId == Guid.Empty)
            {
                return BadRequest("Invalid event ID");
            }

            try
            {
                await _eventService.RemoveEventAsync(EventId);
                return Ok($"Deleted Event {EventId}");
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
