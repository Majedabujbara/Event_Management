using Moq;
using Xunit;
using EventManger.Core.Services;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Core.Domain.DTO;
using System.ComponentModel.DataAnnotations;
using EventManger.Core.Enums;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.Identity;

namespace API_Test
{
    public class EventServiceTest
    {
        private readonly Mock<IEventRepository> _mockEventRepo;
        private readonly EventService _eventService;

        public EventServiceTest()
        {
            _mockEventRepo = new Mock<IEventRepository>();
            _eventService = new EventService(_mockEventRepo.Object);
        }
        [Fact]
        public async Task AddEventAsync_ValidEvent_ReturnsEvent()
        {
            // Arrange
            var eventDto = new EventsRequestDTO
            {
                Name = "Test Event",
                Description = "Test Description",
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(2),
                RoomID = Guid.NewGuid(),
                OrganizationID = Guid.NewGuid(),
                Status = EventStatus.Scheduled,
                PhotoUrl = "test.jpg"
            };

            var expectedEvent = new Event();
            _mockEventRepo.Setup(x => x.AddEventAsync(It.IsAny<Event>()))
                .Callback<Event>(e => expectedEvent = e)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _eventService.AddEventAsync(eventDto);

            // Assert
            Assert.Equal(eventDto.Name, result.Name);
            Assert.Equal(eventDto.Description, result.Description);
            Assert.Equal(eventDto.StartTime, result.StartTime);
            Assert.Equal(eventDto.EndTime, result.EndTime);
            Assert.Equal(eventDto.RoomID, result.RoomID);
            Assert.Equal(eventDto.OrganizationID, result.OrganizationID);
            Assert.Equal(eventDto.Status, result.Status);
            Assert.Equal(eventDto.PhotoUrl, result.PhotoUrl);
        }

        [Fact]
        public async Task AddEventAsync_InvalidTimeRange_ThrowsException()
        {
            // Arrange
            var eventDto = new EventsRequestDTO
            {
                Name = "Test Event",
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(-1), // End before start
                RoomID = Guid.NewGuid()
            };

            _mockEventRepo.Setup(x => x.AddEventAsync(It.IsAny<Event>()))
                .ThrowsAsync(new InvalidOperationException());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _eventService.AddEventAsync(eventDto));
        }

        [Fact]
        public async Task AddEventAsync_PastStartTime_ThrowsException()
        {
            // Arrange
            var eventDto = new EventsRequestDTO
            {
                Name = "Test Event",
                StartTime = DateTime.Now.AddDays(-1), // Past date
                EndTime = DateTime.Now.AddDays(1),
                RoomID = Guid.NewGuid()
            };

            _mockEventRepo.Setup(x => x.AddEventAsync(It.IsAny<Event>()))
                .ThrowsAsync(new InvalidOperationException());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _eventService.AddEventAsync(eventDto));
        }

        [Fact]
        public async Task GetEventAsync_ValidId_ReturnsEvent()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var expectedEvent = new Event { EventID = eventId, Name = "Test Event" };

            _mockEventRepo.Setup(x => x.GetEventAsync(eventId))
                .ReturnsAsync(expectedEvent);

            // Act
            var result = await _eventService.GetEventAsync(eventId);

            // Assert
            Assert.Equal(expectedEvent.EventID, result.EventID);
            Assert.Equal(expectedEvent.Name, result.Name);
        }

        [Fact]
        public async Task GetEventAsync_EmptyGuid_ThrowsException()
        {
            // Arrange
            var emptyGuid = Guid.Empty;
            _mockEventRepo.Setup(x => x.GetEventAsync(emptyGuid))
                .ThrowsAsync(new ArgumentNullException());
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _eventService.GetEventAsync(emptyGuid));
        }

        [Fact]
        public async Task GetEventAsync_NonExistentId_ThrowsException()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            _mockEventRepo.Setup(x => x.GetEventAsync(nonExistentId))
                .ThrowsAsync(new KeyNotFoundException());

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _eventService.GetEventAsync(nonExistentId));
        }
        [Fact]
        public async Task GetAllEventsAsync_ReturnsAllEvents()
        {
            // Arrange
            var events = new List<Event>
    {
        new Event { EventID = Guid.NewGuid(), Name = "Event 1" },
        new Event { EventID = Guid.NewGuid(), Name = "Event 2" }
    };

            _mockEventRepo.Setup(x => x.GetAllEventsAsync())
                .ReturnsAsync(events);

            // Act
            var result = await _eventService.GetAllEventsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllEventsAsync_NoEvents_ReturnsEmptyList()
        {
            // Arrange
            var emptyList = new List<Event>();

            _mockEventRepo.Setup(x => x.GetAllEventsAsync())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _eventService.GetAllEventsAsync();

            // Assert
            Assert.Empty(result);
        }
        [Fact]
        public async Task EditEventAsync_ValidUpdate_UpdatesEvent()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var originalEvent = new Event
            {
                EventID = eventId,
                Name = "Original Name",
                Description = "Original Description"
            };

            var updateDto = new EventsRequestDTO
            {
                Name = "Updated Name",
                Description = "Updated Description",
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(2),
                RoomID = Guid.NewGuid(),
                OrganizationID = Guid.NewGuid(),
                PhotoUrl = "updated.jpg"
            };

            _mockEventRepo.Setup(x => x.GetEventAsync(eventId))
                .ReturnsAsync(originalEvent);

            _mockEventRepo.Setup(x => x.EditEventAsync(It.IsAny<Event>()))
                .Returns(Task.CompletedTask);

            // Act
            await _eventService.EditEventAsync(eventId, updateDto);

            // Assert
            Assert.Equal(updateDto.Name, originalEvent.Name);
            Assert.Equal(updateDto.Description, originalEvent.Description);
            Assert.Equal(updateDto.StartTime, originalEvent.StartTime);
            Assert.Equal(updateDto.EndTime, originalEvent.EndTime);
            Assert.Equal(updateDto.RoomID, originalEvent.RoomID);
            Assert.Equal(updateDto.OrganizationID, originalEvent.OrganizationID);
            Assert.Equal(updateDto.PhotoUrl, originalEvent.PhotoUrl);
        }

        [Fact]
        public async Task EditEventAsync_NonExistentId_ThrowsException()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var updateDto = new EventsRequestDTO();

            _mockEventRepo.Setup(x => x.GetEventAsync(nonExistentId))
                .ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _eventService.EditEventAsync(nonExistentId, updateDto));
        }

        [Fact]
        public async Task EditEventAsync_NullDto_ThrowsException()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _eventService.EditEventAsync(eventId, null));
        }
        [Fact]
        public async Task RemoveEventAsync_ValidId_DeletesEvent()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            _mockEventRepo.Setup(x => x.RemoveEventAsync(eventId))
                .Returns(Task.CompletedTask);

            // Act
            await _eventService.RemoveEventAsync(eventId);

            // Assert
            _mockEventRepo.Verify(x => x.RemoveEventAsync(eventId), Times.Once);
        }

        [Fact]
        public async Task RemoveEventAsync_EmptyGuid_ThrowsException()
        {
            // Arrange
            var emptyGuid = Guid.Empty;
            _mockEventRepo.Setup(x => x.RemoveEventAsync(emptyGuid))
                .ThrowsAsync(new ArgumentNullException());
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _eventService.RemoveEventAsync(emptyGuid));
        }
        [Fact]
        public async Task AddAttendeeAsync_ValidUserAndEvent_AddsAttendee()
        {
            // Arrange
            var user = new ApplicationUser { Id = Guid.NewGuid(), UserName = "testuser" };
            var ev = new Event { EventID = Guid.NewGuid() };

            _mockEventRepo.Setup(x => x.AddAttendeeAsync(user, ev))
                .Returns(Task.CompletedTask);

            // Act
            await _eventService.AddAttendeeAsync(user, ev);

            // Assert
            _mockEventRepo.Verify(x => x.AddAttendeeAsync(user, ev), Times.Once);
        }

        [Fact]
        public async Task DeleteAttendeeAsync_ValidUserAndEvent_RemovesAttendee()
        {
            // Arrange
            var user = new ApplicationUser { Id = Guid.NewGuid(), UserName = "testuser" };
            var ev = new Event { EventID = Guid.NewGuid() };

            _mockEventRepo.Setup(x => x.DeleteAttendeeAsync(user, ev))
                .Returns(Task.CompletedTask);

            // Act
            await _eventService.DeleteAttendeeAsync(user, ev);

            // Assert
            _mockEventRepo.Verify(x => x.DeleteAttendeeAsync(user, ev), Times.Once);
        }

        [Fact]
        public async Task AddAttendeeAsync_InvalidUser_ThrowsException()
        {
            // Arrange
            var invalidUser = new ApplicationUser(); // Missing required fields
            var ev = new Event { EventID = Guid.NewGuid() };

            _mockEventRepo.Setup(x => x.AddAttendeeAsync(invalidUser, ev))
                .ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _eventService.AddAttendeeAsync(invalidUser, ev));
        }
        [Fact]
        public async Task AddEventAsync_EventIdAlreadyExists_ThrowsException()
        {
            // Arrange
            var eventDto = new EventsRequestDTO
            {
                Name = "Test Event",
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(2)
            };

            _mockEventRepo.Setup(x => x.AddEventAsync(It.IsAny<Event>()))
                .ThrowsAsync(new Exception("Event ID Already Exists"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _eventService.AddEventAsync(eventDto));
        }

        [Fact]
        public async Task EditEventAsync_TimeCollision_ThrowsException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var originalEvent = new Event
            {
                EventID = eventId,
                RoomID = Guid.NewGuid(),
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(2)
            };

            var updateDto = new EventsRequestDTO
            {
                RoomID = originalEvent.RoomID,
                StartTime = DateTime.Now.AddDays(1).AddMinutes(30),
                EndTime = DateTime.Now.AddDays(1).AddHours(3)
            };

            _mockEventRepo.Setup(x => x.GetEventAsync(eventId))
                .ReturnsAsync(originalEvent);

            _mockEventRepo.Setup(x => x.EditEventAsync(It.IsAny<Event>()))
                .ThrowsAsync(new InvalidOperationException());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _eventService.EditEventAsync(eventId, updateDto));
        }
    }
}
