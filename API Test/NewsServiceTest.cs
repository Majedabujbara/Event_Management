using EventManger.Core.Services;
using EventManger.Core.Domain.RepositoryContracts;
using Moq;
using Xunit;
using System;
using System.Threading.Tasks;
using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.RepositoryContracts;
using Ganss.Xss;
using System.Diagnostics;
namespace API_Test
{

    public class NewsServiceTest
    {
        private readonly Mock<INewsRepository> _mockRepo;
        private readonly IHtmlSanitizer _htmlSanitizer;
        private readonly NewsService _service;

        public NewsServiceTest()
        {
            _mockRepo = new Mock<INewsRepository>();
            _htmlSanitizer = new HtmlSanitizer(); // Real implementation
            _service = new NewsService(_mockRepo.Object, _htmlSanitizer);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsGuid()
        {
            // Arrange
            var request = new NewsRequestDTO
            {
                Title = "Test Title",
                Content = "<script>alert('xss')</script>Test Content",
                PhotoUrl = "test.jpg"
            };

            _mockRepo.Setup(x => x.AddAsync(It.IsAny<NewsArticle>()))
                .Callback<NewsArticle>(a => a.Id = Guid.NewGuid())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            _mockRepo.Verify(x => x.AddAsync(It.Is<NewsArticle>(a =>
                a.Title == request.Title &&
                !a.Content.Contains("<script>") && // Verify sanitization happened
                a.Content.Contains("Test Content") &&
                a.PhotoUrl == request.PhotoUrl)),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithNullContent_StillCreatesArticle()
        {
            // Arrange
            var request = new NewsRequestDTO
            {
                Title = "Test Title",
                Content = null,
                PhotoUrl = "test.jpg"
            };

            _mockRepo.Setup(x => x.AddAsync(It.IsAny<NewsArticle>()))
                .Callback<NewsArticle>(a => a.Id = Guid.NewGuid())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            _mockRepo.Verify(x => x.AddAsync(It.Is<NewsArticle>(a =>
                a.Title == request.Title &&
                a.Content == string.Empty)), // Should convert null to empty string
                Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsArticle()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedArticle = new NewsArticle { Id = id, Title = "Test" };
            _mockRepo.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(expectedArticle);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.Equal(expectedArticle, result);
            _mockRepo.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ValidArticle_UpdatesWithSanitizedContent()
        {
            // Arrange
            var id = Guid.NewGuid();
            var originalArticle = new NewsArticle
            {
                Id = id,
                Title = "Original",
                Content = "Original Content",
                CreatedDate = DateTime.UtcNow.AddDays(-1)
            };

            var updatedArticle = new NewsArticle
            {
                Id = id,
                Title = "Updated",
                Content = "<script>alert('xss')</script>Updated Content"
            };

            _mockRepo.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(originalArticle);

            // Act
            await _service.UpdateAsync(updatedArticle);

            // Assert
            _mockRepo.Verify(x => x.UpdateAsync(It.Is<NewsArticle>(a =>
                a.Id == id &&
                a.Title == updatedArticle.Title &&
                !a.Content.Contains("<script>") && // Verify sanitization
                a.Content.Contains("Updated Content") &&
                a.CreatedDate == originalArticle.CreatedDate &&
                a.UpdatedDate != null)),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithNullContent_UpdatesWithEmptyString()
        {
            // Arrange
            var id = Guid.NewGuid();
            var originalArticle = new NewsArticle
            {
                Id = id,
                Title = "Original",
                Content = "Original Content",
                CreatedDate = DateTime.UtcNow.AddDays(-1)
            };

            var updatedArticle = new NewsArticle
            {
                Id = id,
                Title = "Updated",
                Content = null
            };

            _mockRepo.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(originalArticle);

            // Act
            await _service.UpdateAsync(updatedArticle);

            // Assert
            _mockRepo.Verify(x => x.UpdateAsync(It.Is<NewsArticle>(a =>
                a.Content == string.Empty)), // Should convert null to empty string
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ValidId_DeletesArticle()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(x => x.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAsync(id);

            // Assert
            _mockRepo.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_With1000Articles_ReturnsQuickly()
        {
            var articles = Enumerable.Range(0, 1000)
                .Select(i => new NewsArticle { Title = $"Article {i}" });

            _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(articles);

            var timer = Stopwatch.StartNew();
            var result = await _service.GetAllAsync();
            timer.Stop();

            Assert.Equal(1000, result.Count());
            Assert.True(timer.ElapsedMilliseconds < 200); // Should complete in <200ms
        }

    }
}