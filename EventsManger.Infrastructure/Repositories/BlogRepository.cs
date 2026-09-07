
using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;

namespace EventManger.Infrastructure.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public BlogRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<BlogPost> AddBlogPostAsync(BlogPost blogPost)
        {
            await _dbContext.blogPosts.AddAsync(blogPost);
            await _dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task AddCommentToBlogPostAsync(Guid blogPostId, Comment comment)
        {
            comment.Id=Guid.NewGuid(); // Ensure the comment has a unique ID

            // Verify blog post exists (more efficient than loading all comments)
            var blogPostExists = await _dbContext.blogPosts.AnyAsync(x => x.BlogId == blogPostId);
            if (!blogPostExists)
            {
                throw new Exception("Blog post not found");
            }

            // Set the relationship
            comment.BlogPostId = blogPostId;

            // Add the comment
            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBlogPostAsync(Guid id)
        {
            if(id == Guid.Empty)
            {
                throw new ArgumentException("Blog post ID cannot be empty", nameof(id));
            }
            BlogPost? post = await _dbContext.blogPosts.FirstOrDefaultAsync(x => x.BlogId == id);
            if(post == null)
            {
                throw new Exception("Blog post not found");
            }
            _dbContext.blogPosts.Remove(post);
            await _dbContext.SaveChangesAsync();

        }

        public async Task DeleteCommentAsync(Guid commentId)
        {
            var comment= await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == commentId);
            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<BlogPost>> GetAllBlogPostsAsync()
        {
            return await _dbContext.blogPosts
                .Include(x => x.Comments)
                .Include(x =>x.Likes)
                .ToListAsync();
        }

        public async Task<BlogPost> GetBlogPostByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Blog post ID cannot be empty", nameof(id));
            }
            //a better error handling should be introduced 
            BlogPost? post = await _dbContext.blogPosts
                .Include(x => x.Comments)
                .Include(x => x.Likes)
                .FirstOrDefaultAsync(x => x.BlogId == id);
            if(post == null)
            {
                throw new Exception("Blog post not found");
            }
            return post;
        }

        public async Task<Comment> GetCommentByIdAsync(Guid commentId)
        {
            return await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == commentId);
        }

        public async Task<List<Comment?>?> GetCommentsByBlogPostIdAsync(Guid blogPostId)
        {
            BlogPost? post = await _dbContext.blogPosts
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.BlogId == blogPostId);
            if(post == null)
            {
                throw new Exception("Blog post not found");
            }
            return post.Comments;
        }

        public async Task UpdateBlogPostAsync(BlogPost blogPost)
        {
            _dbContext.blogPosts.Update(blogPost);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            _dbContext.Comments.Update(comment);
            await _dbContext.SaveChangesAsync();
        }
    }
}
