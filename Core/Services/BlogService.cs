using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Core.ServicesContracts;
using Ganss.Xss;

namespace EventManger.Core.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IHtmlSanitizer _htmlSanitizer;
        private readonly IPersonRepository _personRepository;

        public BlogService(IBlogRepository blogRepoitory, IHtmlSanitizer htmlSanitizer, IPersonRepository personRepository)
        {
            _blogRepository = blogRepoitory;
            _htmlSanitizer = htmlSanitizer;
            _personRepository = personRepository;
        }
        public async Task<BlogPost> AddBlogPostAsync(BlogPostRequestDTO blogPost)
        {
            var sanitizedContent = _htmlSanitizer.Sanitize(blogPost.Content);
            var sanitizedTitle = _htmlSanitizer.Sanitize(blogPost.Title);

            if (blogPost == null)
            {
                throw new ArgumentNullException(nameof(blogPost), "Blog post cannot be null");
            }
            BlogPost blogPostEntity = new BlogPost
            {
                Title = sanitizedTitle,
                Content = sanitizedContent,
                UserId = (Guid)blogPost.UserId,
                UserName = blogPost.UserName,
                TimePosted = blogPost.TimePosted,
                Comments = new List<Comment>(),
                ImagePath = blogPost.ImagePath
            };
            return await _blogRepository.AddBlogPostAsync(blogPostEntity);
        }

        public async Task AddCommentToBlogPostAsync(Guid blogPostId, Comment comment)
        {

            await _blogRepository.AddCommentToBlogPostAsync(blogPostId, comment);
        }

        public async Task DeleteBlogPostAsync(Guid id)
        {
            await _blogRepository.DeleteBlogPostAsync(id);
        }

        public async Task DeleteCommentAsync(Guid commentId)
        {
            await _blogRepository.DeleteCommentAsync(commentId);
        }

        public async Task<List<BlogPost>> GetAllBlogPostsAsync()
        {
            return await _blogRepository.GetAllBlogPostsAsync();
        }

        public async Task<BlogPost> GetBlogPostByIdAsync(Guid id)
        {
            return await _blogRepository.GetBlogPostByIdAsync(id);
        }

        public async Task<Comment> GetCommentByIdAsync(Guid commentId)
        {
            return await _blogRepository.GetCommentByIdAsync(commentId);
        }

        public async Task<List<Comment>> GetCommentsByBlogPostIdAsync(Guid blogPostId)
        {
            return await _blogRepository.GetCommentsByBlogPostIdAsync(blogPostId);
        }

        public async Task UpdateBlogPostAsync(Guid blogPostId, BlogPostRequestDTO blogPost)
        {
            var sanitizedContent = _htmlSanitizer.Sanitize(blogPost.Content);
            var sanitizedTitle = _htmlSanitizer.Sanitize(blogPost.Title);

            if (blogPost == null)
            {
                throw new ArgumentNullException(nameof(blogPost), "Blog post cannot be null");
            }

            var existingPost = await _blogRepository.GetBlogPostByIdAsync(blogPostId);
            if (existingPost == null)
            {
                throw new ArgumentException($"Blog post with ID {blogPostId} not found");
            }

            existingPost.Title = sanitizedTitle;
            existingPost.Content = sanitizedContent;
            existingPost.ImagePath = blogPost.ImagePath;

            await _blogRepository.UpdateBlogPostAsync(existingPost);
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            var sanitizedContent = _htmlSanitizer.Sanitize(comment.Content);
            comment.Content = sanitizedContent;
            await _blogRepository.UpdateCommentAsync(comment);
        }
        public async Task LikeBlogPostAsync(Guid blogPostId, Guid userId)
        {
            var blogPost = await _blogRepository.GetBlogPostByIdAsync(blogPostId);
            if (blogPost == null)
                throw new ArgumentException($"Blog post with ID {blogPostId} not found");

            var user = await _personRepository.GetPersonAsync(userId);
            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found");

            if (!blogPost.Likes.Any(u => u.Id == userId))
            {
                blogPost.Likes.Add(user);
                await _blogRepository.UpdateBlogPostAsync(blogPost);
            }
        }

        public async Task UnlikeBlogPostAsync(Guid blogPostId, Guid userId)
        {
            var blogPost = await _blogRepository.GetBlogPostByIdAsync(blogPostId);
            if (blogPost == null)
                throw new ArgumentException($"Blog post with ID {blogPostId} not found");

            var userToRemove = blogPost.Likes.FirstOrDefault(u => u.Id == userId);
            if (userToRemove != null)
            {
                blogPost.Likes.Remove(userToRemove);
                await _blogRepository.UpdateBlogPostAsync(blogPost);
            }
        }

    }
}
