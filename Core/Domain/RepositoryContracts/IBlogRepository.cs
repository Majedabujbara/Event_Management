using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;

namespace EventManger.Core.Domain.RepositoryContracts
{
    public interface IBlogRepository
    {
        Task<List<BlogPost>> GetAllBlogPostsAsync();
        Task<BlogPost> GetBlogPostByIdAsync(Guid id);
        Task<BlogPost> AddBlogPostAsync(BlogPost blogPost);
        Task UpdateBlogPostAsync(BlogPost blogPost);
        Task DeleteBlogPostAsync(Guid id);
        Task<List<Comment>> GetCommentsByBlogPostIdAsync(Guid blogPostId);
        Task<Comment> GetCommentByIdAsync(Guid commentId);
        Task AddCommentToBlogPostAsync(Guid blogPostId, Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(Guid commentId);
    }
}
