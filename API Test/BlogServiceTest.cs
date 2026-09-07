using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Core.Services;
using EventManger.Core.ServicesContracts;
using Ganss.Xss;
using Moq;

namespace API_Test
{
    public class BlogServiceTest
    {
        private readonly IHtmlSanitizer _htmlSanitizer;
        private readonly IBlogService _blogService;
        private readonly IPersonRepository _personRepository;
        public BlogServiceTest()
        {
            var mockBlogRepository = new Mock<IBlogRepository>();
            var mockPersonRepository = new Mock<IPersonRepository>();
            _blogService = new BlogService(mockBlogRepository.Object, _htmlSanitizer, mockPersonRepository.Object);
        }
        [Fact]
        public async Task AddBlogPostAsync_Throws_When_Null()
        {
            // Arrange
            var mockRepo = new Mock<IBlogRepository>();
            var mockPersonRepository = new Mock<IPersonRepository>();
            var service = new BlogService(mockRepo.Object, _htmlSanitizer, mockPersonRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() =>
                service.AddBlogPostAsync(null)
            );
        }

        [Fact]
        public async Task AddBlogPostAsync_Returns_BlogPost_When_Valid()
        {
            // Arrange
            var mockRepo = new Mock<IBlogRepository>();
            var mockPersonRepository = new Mock<IPersonRepository>();

            var service = new BlogService(mockRepo.Object, _htmlSanitizer, mockPersonRepository.Object);

            var request = new BlogPostRequestDTO
            {
                Title = "Test Title",
                Content = "Test Content",
                UserId = Guid.NewGuid(),
                UserName = "TestUser",
                TimePosted = DateTime.UtcNow,
                ImagePath = "test.jpg"
            };

            var expectedBlogPost = new BlogPost
            {
                Title = request.Title,
                Content = request.Content,
                UserId = (Guid)request.UserId,
                UserName = request.UserName,
                TimePosted = request.TimePosted,
                ImagePath = request.ImagePath
            };

            mockRepo.Setup(x => x.AddBlogPostAsync(It.IsAny<BlogPost>()))
                    .ReturnsAsync(expectedBlogPost);

            // Act
            var result = await service.AddBlogPostAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Title, result.Title);
            mockRepo.Verify(x => x.AddBlogPostAsync(It.IsAny<BlogPost>()), Times.Once);
        }
        [Fact]
        public async Task GetBlogPostByIdAsync_Returns_BlogPost_When_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IBlogRepository>();
            var mockPersonRepository = new Mock<IPersonRepository>();

            var service = new BlogService(mockRepo.Object, _htmlSanitizer, mockPersonRepository.Object);

            var blogPostId = Guid.NewGuid();
            var expectedBlogPost = new BlogPost { BlogId = blogPostId, Title = "Test" };

            mockRepo.Setup(x => x.GetBlogPostByIdAsync(blogPostId))
                    .ReturnsAsync(expectedBlogPost);

            // Act
            var result = await service.GetBlogPostByIdAsync(blogPostId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(blogPostId, result.BlogId);
        }

        [Fact]
        public async Task GetBlogPostByIdAsync_Returns_Null_When_Not_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IBlogRepository>();
            var mockPersonRepository = new Mock<IPersonRepository>();

            var service = new BlogService(mockRepo.Object, _htmlSanitizer, mockPersonRepository.Object);

            mockRepo.Setup(x => x.GetBlogPostByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((BlogPost?)null);

            // Act
            var result = await service.GetBlogPostByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task UpdateBlogPostAsync_Throws_When_Null()
        {
            // Arrange
            var mockRepo = new Mock<IBlogRepository>();
            var mockPersonRepository = new Mock<IPersonRepository>();

            var service = new BlogService(mockRepo.Object, _htmlSanitizer, mockPersonRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                service.UpdateBlogPostAsync(Guid.NewGuid(), null)
            );
        }

        [Fact]
        public async Task UpdateBlogPostAsync_Throws_When_Post_Not_Found()
        {
            // Arrange
            var mockRepo = new Mock<IBlogRepository>();
            var mockPersonRepository = new Mock<IPersonRepository>();

            var service = new BlogService(mockRepo.Object, _htmlSanitizer, mockPersonRepository.Object);

            mockRepo.Setup(x => x.GetBlogPostByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((BlogPost?)null);

            var request = new BlogPostRequestDTO { Title = "Updated Title" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.UpdateBlogPostAsync(Guid.NewGuid(), request)
            );
        }

        [Fact]
        public async Task UpdateBlogPostAsync_Updates_When_Valid()
        {
            // Arrange
            var mockRepo = new Mock<IBlogRepository>();
            var mockPersonRepository = new Mock<IPersonRepository>();

            var service = new BlogService(mockRepo.Object, _htmlSanitizer, mockPersonRepository.Object);

            var blogPostId = Guid.NewGuid();
            var existingPost = new BlogPost { BlogId = blogPostId, Title = "Old Title" };

            mockRepo.Setup(x => x.GetBlogPostByIdAsync(blogPostId))
                    .ReturnsAsync(existingPost);

            var request = new BlogPostRequestDTO
            {
                Title = "New Title",
                Content = "New Content",
                ImagePath = "new.jpg"
            };

            // Act
            await service.UpdateBlogPostAsync(blogPostId, request);

            // Assert
            Assert.Equal("New Title", existingPost.Title);
            Assert.Equal("New Content", existingPost.Content);
            mockRepo.Verify(x => x.UpdateBlogPostAsync(existingPost), Times.Once);
        }
        [Fact]
        public async Task DeleteBlogPostAsync_Deletes_When_Valid()
        {
            // Arrange
            var mockRepo = new Mock<IBlogRepository>();
            var mockPersonRepository = new Mock<IPersonRepository>();

            var service = new BlogService(mockRepo.Object, _htmlSanitizer, mockPersonRepository.Object);

            var blogPostId = Guid.NewGuid();

            // Act
            await service.DeleteBlogPostAsync(blogPostId);

            // Assert
            mockRepo.Verify(x => x.DeleteBlogPostAsync(blogPostId), Times.Once);
        }
        [Fact]
        public async Task DeleteBlogPostAsync_Throws_When_Post_Not_Found()
        {
            // Arrange
            var mockRepo = new Mock<IBlogRepository>();
            var mockPersonRepository = new Mock<IPersonRepository>();

            var service = new BlogService(mockRepo.Object, _htmlSanitizer, mockPersonRepository.Object);

            var invalidBlogPostId = Guid.NewGuid();

            // Mock the repository to return null (post doesn't exist)
            mockRepo.Setup(x => x.DeleteBlogPostAsync(invalidBlogPostId)).ThrowsAsync(new Exception("Blog post not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                service.DeleteBlogPostAsync(invalidBlogPostId)
            );
        }

        [Fact]
        public async Task AddCommentToBlogPostAsync_Adds_Comment_When_Valid()
        {
            // Arrange
            var mockRepo = new Mock<IBlogRepository>();
            var mockPersonRepository = new Mock<IPersonRepository>();

            var service = new BlogService(mockRepo.Object, _htmlSanitizer, mockPersonRepository.Object);

            var blogPostId = Guid.NewGuid();
            var comment = new Comment { Content = "Test Comment" };

            // Act
            await service.AddCommentToBlogPostAsync(blogPostId, comment);

            // Assert
            mockRepo.Verify(x => x.AddCommentToBlogPostAsync(blogPostId, comment), Times.Once);
        }
        [Fact]
        public async Task AddCommentToBlogPostAsync_Throws_When_Comment_Null()
        {
            // Arrange
            var mockRepo = new Mock<IBlogRepository>();
            var mockPersonRepository = new Mock<IPersonRepository>();

            var service = new BlogService(mockRepo.Object, _htmlSanitizer, mockPersonRepository.Object);

            var blogPostId = Guid.NewGuid();
            mockRepo.Setup(x => x.AddCommentToBlogPostAsync(blogPostId,null)).ThrowsAsync(new ArgumentNullException("Blog post not found"));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                service.AddCommentToBlogPostAsync(blogPostId, null!)
            );

        }
        [Fact]
        public async Task AddCommentToBlogPostAsync_Throws_When_BlogPost_Not_Found()
        {
            // Arrange
            var mockRepo = new Mock<IBlogRepository>();
            var mockPersonRepository = new Mock<IPersonRepository>();

            var service = new BlogService(mockRepo.Object, _htmlSanitizer, mockPersonRepository.Object);

            var invalidBlogPostId = Guid.NewGuid();
            var comment = new Comment { Content = "Test Comment" };

            // Mock the repository to return null (post doesn't exist)
            mockRepo.Setup(x => x.AddCommentToBlogPostAsync(invalidBlogPostId,comment))
                    .ThrowsAsync(new Exception("Blog post not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                service.AddCommentToBlogPostAsync(invalidBlogPostId, comment)
            );

        }
    }
}