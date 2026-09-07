using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Services;
using EventManger.Core.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManger.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IWebHostEnvironment _env;
        public BlogsController(IWebHostEnvironment env,IBlogService blogService)
        {
            _blogService = blogService;
            _env = env;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPosts = await _blogService.GetAllBlogPostsAsync();
            return Ok(blogPosts);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBlogPostById(Guid id)
        {
            var blogPost = await _blogService.GetBlogPostByIdAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            return Ok(blogPost);
        }

        [HttpPost]
        public async Task<IActionResult> AddBlogPost([FromForm] BlogPostRequestDTO blogPost)
        {
            if (blogPost == null)
            {
                return BadRequest();
            }

            if (blogPost.Image != null)
            {
                var blogFolder = Path.Combine(_env.WebRootPath, "blog-images");
                if (!Directory.Exists(blogFolder)) Directory.CreateDirectory(blogFolder);

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(blogPost.Image.FileName)}";
                var filePath = Path.Combine(blogFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await blogPost.Image.CopyToAsync(stream);
                }

                blogPost.ImagePath = $"/blog-images/{uniqueFileName}";
            }

            blogPost.UserId= Guid.Parse(User.Identities.First().Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            blogPost.UserName = User.Identities.First().Claims.First(x=>x.Type=="name").Value;
            await _blogService.AddBlogPostAsync(blogPost);
            return Ok(blogPost);
        }

        [HttpPatch("{blogPostId}")]
        public async Task<IActionResult> UpdateBlogPost(Guid blogPostId, [FromForm] BlogPostRequestDTO blogPost)
        {
            if (blogPost == null)
            {
                return BadRequest("Blog post data cannot be null");
            }

            try
            {

                if (blogPost.Image != null)
                {
                    var blogFolder = Path.Combine(_env.WebRootPath, "blog-images");
                    if (!Directory.Exists(blogFolder)) Directory.CreateDirectory(blogFolder);

                    var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(blogPost.Image.FileName)}";
                    var filePath = Path.Combine(blogFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await blogPost.Image.CopyToAsync(stream);
                    }

                    blogPost.ImagePath = $"/blog-images/{uniqueFileName}";
                }

                await _blogService.UpdateBlogPostAsync(blogPostId, blogPost);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the blog post");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogPost(Guid id)
        {
            var blogPost = await _blogService.GetBlogPostByIdAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            await _blogService.DeleteBlogPostAsync(id);
            return NoContent();
        }
        [HttpPost("{id}/like")]
        public async Task<IActionResult> LikePost(Guid id, [FromBody] Guid userId)
        {
            await _blogService.LikeBlogPostAsync(id, userId);
            return Ok(new { message = "Post liked successfully" });
        }

        [HttpPost("{id}/unlike")]
        public async Task<IActionResult> UnlikePost(Guid id, [FromBody] Guid userId)
        {
            await _blogService.UnlikeBlogPostAsync(id, userId);
            return Ok(new { message = "Post unliked successfully" });
        }

        [HttpGet("{blogPostId}/likes")]
        public async Task<IActionResult> GetLikesByBlogPostId(Guid blogPostId)
        {
            var likes = await _blogService.GetBlogPostByIdAsync(blogPostId);

            return Ok(likes.Likes.Count);
        }

    }

}
