using EventManger.Core.Domain.Entites;
using EventManger.Core.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManger.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CommentsController : ControllerBase
    {
        private readonly IBlogService _blogService;
        public CommentsController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        [HttpPost("{Id}")]
        public async Task<IActionResult> AddCommentToPost(Guid Id, [FromBody] Comment comment)
        {
            if (comment == null)
            {
                return BadRequest();
            }
            comment.BlogPostId = Id;
            comment.BlogPost = _blogService.GetBlogPostByIdAsync(Id).Result;
            if(comment.BlogPost == null)
            {
                return NotFound();
            }
            comment.UserId = Guid.Parse(User.Identities.First().Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            comment.CommentatorName = User.Identities.First().Claims.First(x => x.Type == "name").Value;
            await _blogService.AddCommentToBlogPostAsync(Id, comment);
            return Ok(comment);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCommentsByBlogPostId(Guid Id)
        {
            var comments = await _blogService.GetCommentsByBlogPostIdAsync(Id);
            if (comments == null)
            {
                return NotFound();
            }
            return Ok(comments);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteComment(Guid Id)
        {
            var comment = await _blogService.GetCommentByIdAsync(Id);
            if (comment == null)
            {
                return NotFound();
            }
            await _blogService.DeleteCommentAsync(Id);
            return NoContent();
        }
    }
}
