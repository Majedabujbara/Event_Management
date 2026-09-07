using EventManger.Core.Domain.Entities;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Core.Services;
using Moq;

namespace API_Test
{
    public class OrganizationServiceTest
    {
        private readonly Mock<IOrganizationRepository> _mockRepo;
        private readonly OrganizationService _service;

        public OrganizationServiceTest()
        {
            _mockRepo = new Mock<IOrganizationRepository>();
            _service = new OrganizationService(_mockRepo.Object);
        }
        [Fact]
        public async Task GetAllOrganizationsAsync_ShouldReturnAllOrganizations()
        {
            // Arrange
            var organizations = new List<Organization>
            {
                new Organization { OrganizationID = Guid.NewGuid(), Name = "Org 1" },
                new Organization { OrganizationID = Guid.NewGuid(), Name = "Org 2" }
            };

            _mockRepo.Setup(x => x.GetAllOrganizationsAsync()).ReturnsAsync(organizations);

            // Act
            var result = await _service.GetAllOrganizationsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            _mockRepo.Verify(x => x.GetAllOrganizationsAsync(), Times.Once);
        }
        [Fact]
        public async Task GetOrganizationAsync_WithValidId_ShouldReturnOrganization()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var expectedOrg = new Organization { OrganizationID = orgId, Name = "Test Org" };

            _mockRepo.Setup(x => x.GetOrganizationAsync(orgId)).ReturnsAsync(expectedOrg);

            // Act
            var result = await _service.GetOrganizationAsync(orgId);

            // Assert
            Assert.Equal(expectedOrg, result);
            _mockRepo.Verify(x => x.GetOrganizationAsync(orgId), Times.Once);
        }
        [Fact]
        public async Task GetOrganizationAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            _mockRepo.Setup(x => x.GetOrganizationAsync(orgId)).ReturnsAsync((Organization)null);

            // Act
            var result = await _service.GetOrganizationAsync(orgId);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task AddOrganizationAsync_WithValidData_ShouldAddOrganization()
        {
            // Arrange
            var orgDto = new OrganizationRequestDTO
            {
                Name = "New Org",
                Description = "Description",
                College = "College",
                ContactNumber = "1234567890",
                Email = "org@example.com",
                LogoUrl = "http://example.com/logo.png"
            };

            Organization addedOrg = null;
            _mockRepo.Setup(x => x.AddOrganizationAsync(It.IsAny<Organization>()))
                .Callback<Organization>(o => addedOrg = o)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.AddOrganizationAsync(orgDto);

            // Assert
            _mockRepo.Verify(x => x.AddOrganizationAsync(It.IsAny<Organization>()), Times.Once);
            Assert.Equal(orgDto.Name, result.Name);
            Assert.Equal(orgDto.Description, result.Description);
            Assert.Equal(orgDto.College, result.College);
            Assert.Equal(orgDto.ContactNumber, result.ContactNumber);
            Assert.Equal(orgDto.Email, result.Email);
            Assert.Equal(orgDto.LogoUrl, result.LogoUrl);
            Assert.NotEqual(Guid.Empty, result.OrganizationID);
        }
        [Fact]
        public async Task RemoveOrganizationAsync_WithValidId_ShouldCallRepository()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            _mockRepo.Setup(x => x.RemoveOrganizationAsync(orgId)).Returns(Task.CompletedTask);

            // Act
            await _service.RemoveOrganizationAsync(orgId);

            // Assert
            _mockRepo.Verify(x => x.RemoveOrganizationAsync(orgId), Times.Once);
        }
        [Fact]
        public async Task EditOrganizationAsync_WithValidData_ShouldUpdateOrganization()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var existingOrg = new Organization
            {
                OrganizationID = orgId,
                Name = "Old Name",
                Description = "Old Desc",
                College = "Old College",
                ContactNumber = "1111111111",
                Email = "old@example.com",
                LogoUrl = "http://old.com/logo.png"
            };

            var orgDto = new OrganizationRequestDTO
            {
                Name = "New Name",
                Description = "New Desc",
                College = "New College",
                ContactNumber = "2222222222",
                Email = "new@example.com",
                LogoUrl = "http://new.com/logo.png"
            };

            _mockRepo.Setup(x => x.GetOrganizationAsync(orgId)).ReturnsAsync(existingOrg);
            _mockRepo.Setup(x => x.EditOrganizationAsync(It.IsAny<Organization>())).Returns(Task.CompletedTask);

            // Act
            await _service.EditOrganizationAsync(orgId, orgDto);

            // Assert
            _mockRepo.Verify(x => x.GetOrganizationAsync(orgId), Times.Once);
            _mockRepo.Verify(x => x.EditOrganizationAsync(existingOrg), Times.Once);

            Assert.Equal(orgDto.Name, existingOrg.Name);
            Assert.Equal(orgDto.Description, existingOrg.Description);
            Assert.Equal(orgDto.College, existingOrg.College);
            Assert.Equal(orgDto.ContactNumber, existingOrg.ContactNumber);
            Assert.Equal(orgDto.Email, existingOrg.Email);
            Assert.Equal(orgDto.LogoUrl, existingOrg.LogoUrl);
        }
        [Fact]
        public async Task EditOrganizationAsync_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var orgDto = new OrganizationRequestDTO { /* valid data */ };

            _mockRepo.Setup(x => x.GetOrganizationAsync(orgId)).ReturnsAsync((Organization)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.EditOrganizationAsync(orgId, orgDto));
            _mockRepo.Verify(x => x.GetOrganizationAsync(orgId), Times.Once);
            _mockRepo.Verify(x => x.EditOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }
        [Fact]
        public async Task EditOrganizationAsync_WhenUpdateFails_ShouldThrowException()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var existingOrg = new Organization { OrganizationID = orgId };
            var orgDto = new OrganizationRequestDTO { /* valid data */ };

            _mockRepo.Setup(x => x.GetOrganizationAsync(orgId)).ReturnsAsync(existingOrg);
            _mockRepo.Setup(x => x.EditOrganizationAsync(It.IsAny<Organization>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.EditOrganizationAsync(orgId, orgDto));
        }
    }
}
