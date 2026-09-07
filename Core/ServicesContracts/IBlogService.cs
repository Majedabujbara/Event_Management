using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;

namespace EventManger.Core.ServicesContracts
{
    public interface IBlogService
    {
        Task<List<BlogPost>> GetAllBlogPostsAsync();
        Task<BlogPost> GetBlogPostByIdAsync(Guid id);
        Task<BlogPost> AddBlogPostAsync(BlogPostRequestDTO blogPost);
        Task UpdateBlogPostAsync(Guid blogPostId, BlogPostRequestDTO blogPost);
        Task DeleteBlogPostAsync(Guid id);
        Task<List<Comment>> GetCommentsByBlogPostIdAsync(Guid blogPostId);
        Task<Comment> GetCommentByIdAsync(Guid commentId);
        Task AddCommentToBlogPostAsync(Guid blogPostId, Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(Guid commentId);
        Task LikeBlogPostAsync(Guid blogPostId, Guid userId);
        Task UnlikeBlogPostAsync(Guid blogPostId, Guid userId);
    }
}
