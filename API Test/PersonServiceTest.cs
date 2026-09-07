using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManger.Core.Domain.Identity;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Core.Services;
using EventManger.Core.ServicesContracts;
using Moq;
using Xunit;

namespace API_Test
{
    public class PersonServiceTest
    {
        private readonly Mock<IPersonRepository> _personRepositoryMock;
        private readonly IPersonService _personService;
        private readonly Guid _testId = Guid.NewGuid();

        public PersonServiceTest()
        {
            _personRepositoryMock = new Mock<IPersonRepository>();
            _personService = new PersonService(_personRepositoryMock.Object);
        }

        #region GetAllPersonAsync Tests

        [Fact]
        public async Task GetAllPersonAsync_ReturnsAllPersons()
        {
            // Arrange
            var expectedPersons = new List<ApplicationUser>
            {
                new ApplicationUser { Id = Guid.NewGuid(), UserName = "user1@test.com" },
                new ApplicationUser { Id = Guid.NewGuid(), UserName = "user2@test.com" }
            };

            _personRepositoryMock.Setup(x => x.GetAllPersonAsync())
                .ReturnsAsync(expectedPersons);

            // Act
            var result = await _personService.GetAllPersonAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _personRepositoryMock.Verify(x => x.GetAllPersonAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllPersonAsync_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            _personRepositoryMock.Setup(x => x.GetAllPersonAsync())
                .ReturnsAsync(new List<ApplicationUser>());

            // Act
            var result = await _personService.GetAllPersonAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetPersonAsync Tests

        [Fact]
        public async Task GetPersonAsync_ValidId_ReturnsPerson()
        {
            // Arrange
            var expectedPerson = new ApplicationUser
            {
                Id = _testId,
                UserName = "test@user.com"
            };

            _personRepositoryMock.Setup(x => x.GetPersonAsync(_testId))
                .ReturnsAsync(expectedPerson);

            // Act
            var result = await _personService.GetPersonAsync(_testId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_testId, result.Id);
            Assert.Equal(expectedPerson.UserName, result.UserName);
            _personRepositoryMock.Verify(x => x.GetPersonAsync(_testId), Times.Once);
        }

        [Fact]
        public async Task GetPersonAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            _personRepositoryMock.Setup(x => x.GetPersonAsync(It.IsAny<Guid>()))
                .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _personService.GetPersonAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region DeletePersonAsync Tests

        [Fact]
        public async Task DeletePersonAsync_ValidId_DeletesPerson()
        {
            // Arrange
            _personRepositoryMock.Setup(x => x.DeletePersonAsync(_testId))
                .Returns(Task.CompletedTask);

            // Act
            await _personService.DeletePersonAsync(_testId);

            // Assert
            _personRepositoryMock.Verify(x => x.DeletePersonAsync(_testId), Times.Once);
        }

        [Fact]
        public async Task DeletePersonAsync_EmptyGuid_ThrowsArgumentException()
        {
            // Arrange
            _personRepositoryMock.Setup(x => x.DeletePersonAsync(It.IsAny<Guid>()))
                .Throws(new ArgumentException("Id cannot be empty"));
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _personService.DeletePersonAsync(Guid.Empty));
        }

        #endregion

        #region EditPersonAsync Tests

        [Fact]
        public async Task EditPersonAsync_ValidPerson_UpdatesPerson()
        {
            // Arrange
            var personToUpdate = new ApplicationUser
            {
                Id = _testId,
                UserName = "old@email.com",
                Email = "old@email.com"
            };

            var updatedPerson = new ApplicationUser
            {
                Id = _testId,
                UserName = "new@email.com",
                Email = "new@email.com"
            };

            _personRepositoryMock.Setup(x => x.EditPersonAsync(It.IsAny<ApplicationUser>()))
                .Returns(Task.CompletedTask);

            // Act
            await _personService.EditPersonAsync(updatedPerson);

            // Assert
            _personRepositoryMock.Verify(x => x.EditPersonAsync(updatedPerson), Times.Once);
        }

        [Fact]
        public async Task EditPersonAsync_NullPerson_ThrowsArgumentNullException()
        {
            _personRepositoryMock.Setup(x => x.EditPersonAsync(It.IsAny<ApplicationUser>()))
                .Throws(new ArgumentNullException(nameof(ApplicationUser)));
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _personService.EditPersonAsync(null));
        }

        #endregion
    }
}