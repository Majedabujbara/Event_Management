using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Core.Services;
using EventManger.Core.ServicesContracts;
using Moq;
using Xunit;

namespace API_Test
{
    public class RoomServiceTest
    {
        private readonly Mock<IRoomRepository> _roomRepositoryMock;
        private readonly IRoomService _roomService;
        private readonly Guid _testId = Guid.NewGuid();

        public RoomServiceTest()
        {
            _roomRepositoryMock = new Mock<IRoomRepository>();
            _roomService = new RoomService(_roomRepositoryMock.Object);
        }

        #region AddRoomAsync Tests

        [Fact]
        public async Task AddRoomAsync_ValidRequest_ReturnsAddedRoom()
        {
            // Arrange
            var request = new RoomRequestDTO
            {
                Name = "Test Room",
                Description = "Test Description",
                Seats = 50
            };

            var expectedRoom = new Room
            {
                Name = request.Name,
                Description = request.Description,
                Seats = request.Seats
            };

            _roomRepositoryMock.Setup(x => x.AddRoomAsync(It.IsAny<Room>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _roomService.AddRoomAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Name, result.Name);
            Assert.Equal(request.Description, result.Description);
            Assert.Equal(request.Seats, result.Seats);
            Assert.NotEqual(Guid.Empty, result.RoomID);
            _roomRepositoryMock.Verify(x => x.AddRoomAsync(It.IsAny<Room>()), Times.Once);
        }

        #endregion

        #region GetAllRoomsAsync Tests

        [Fact]
        public async Task GetAllRoomsAsync_ReturnsAllRooms()
        {
            // Arrange
            var expectedRooms = new List<Room>
            {
                new Room { RoomID = Guid.NewGuid(), Name = "Room 1", Seats = 20 },
                new Room { RoomID = Guid.NewGuid(), Name = "Room 2", Seats = 30 }
            };

            _roomRepositoryMock.Setup(x => x.GetAllRoomsAsync())
                .ReturnsAsync(expectedRooms);

            // Act
            var result = await _roomService.GetAllRoomsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _roomRepositoryMock.Verify(x => x.GetAllRoomsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllRoomsAsync_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            _roomRepositoryMock.Setup(x => x.GetAllRoomsAsync())
                .ReturnsAsync(new List<Room>());

            // Act
            var result = await _roomService.GetAllRoomsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetRoomByIDAsync Tests

        [Fact]
        public async Task GetRoomByIDAsync_ValidId_ReturnsRoom()
        {
            // Arrange
            var expectedRoom = new Room { RoomID = _testId, Name = "Test Room", Seats = 50 };

            _roomRepositoryMock.Setup(x => x.GetRoomByIDAsync(_testId))
                .ReturnsAsync(expectedRoom);

            // Act
            var result = await _roomService.GetRoomByIDAsync(_testId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_testId, result.RoomID);
            Assert.Equal(expectedRoom.Name, result.Name);
            _roomRepositoryMock.Verify(x => x.GetRoomByIDAsync(_testId), Times.Once);
        }

        [Fact]
        public async Task GetRoomByIDAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            _roomRepositoryMock.Setup(x => x.GetRoomByIDAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Room)null);

            // Act
            var result = await _roomService.GetRoomByIDAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region DeleteRoomAsync Tests

        [Fact]
        public async Task DeleteRoomAsync_ValidId_DeletesRoom()
        {
            // Arrange
            _roomRepositoryMock.Setup(x => x.DeleteRoomAsync(_testId))
                .Returns(Task.CompletedTask);

            // Act
            await _roomService.DeleteRoomAsync(_testId);

            // Assert
            _roomRepositoryMock.Verify(x => x.DeleteRoomAsync(_testId), Times.Once);
        }

        #endregion

        #region EditRoomAsync Tests

        [Fact]
        public async Task EditRoomAsync_ValidRequest_UpdatesRoom()
        {
            // Arrange
            var existingRoom = new Room
            {
                RoomID = _testId,
                Name = "Old Name",
                Description = "Old Description",
                Seats = 20
            };

            var request = new RoomRequestDTO
            {
                Name = "New Name",
                Description = "New Description",
                Seats = 30
            };

            _roomRepositoryMock.Setup(x => x.GetRoomByIDAsync(_testId))
                .ReturnsAsync(existingRoom);

            _roomRepositoryMock.Setup(x => x.EditRoomAsync(It.IsAny<Room>()))
                .Returns(Task.CompletedTask);

            // Act
            await _roomService.EditRoomAsync(_testId, request);

            // Assert
            Assert.Equal(request.Name, existingRoom.Name);
            Assert.Equal(request.Description, existingRoom.Description);
            Assert.Equal(request.Seats, existingRoom.Seats);
            _roomRepositoryMock.Verify(x => x.EditRoomAsync(existingRoom), Times.Once);
        }

        [Fact]
        public async Task EditRoomAsync_NonExistentRoom_ThrowsException()
        {
            // Arrange
            var request = new RoomRequestDTO
            {
                Name = "Test Room",
                Seats = 50
            };

            _roomRepositoryMock.Setup(x => x.GetRoomByIDAsync(_testId))
                .ReturnsAsync((Room)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _roomService.EditRoomAsync(_testId, request));
            Assert.Equal("No room with such an ID exists to be edited", exception.Message);
        }

        #endregion
    }
}