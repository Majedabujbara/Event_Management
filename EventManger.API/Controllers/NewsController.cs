using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;
using EventManger.Core.ServicesContracts;
using EventManger.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManger.API.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly IWebHostEnvironment _env;
        public NewsController(INewsService newsService,IWebHostEnvironment env)
        {
            _newsService = newsService;
            _env = env;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<NewsArticle>>> GetAllNewsArticles()
        {
            try
            {
                var articles = await _newsService.GetAllAsync();
                return Ok(articles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<NewsArticle>> GetNewsArticleById(Guid id)
        {
            try
            {
                var article = await _newsService.GetByIdAsync(id);
                return Ok(article);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPost]
        public async Task<ActionResult<NewsArticle>> CreateNewsArticle([FromForm]NewsRequestDTO create)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (create.NewsPhoto != null)
            {
                var newsFolder = Path.Combine(_env.WebRootPath, "news-photos");
                if (!Directory.Exists(newsFolder)) Directory.CreateDirectory(newsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(create.NewsPhoto.FileName)}";
                var filePath = Path.Combine(newsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await create.NewsPhoto.CopyToAsync(stream);
                }

                create.PhotoUrl = $"/news-photos/{uniqueFileName}";
            }

            try
            {
                var id = await _newsService.CreateAsync(create);
                var article = await _newsService.GetByIdAsync(id);

                return CreatedAtAction(
                    nameof(GetNewsArticleById),
                    new { id = article.Id },
                    article);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateNewsArticle(Guid id,[FromForm] NewsRequestDTO update)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (update.NewsPhoto != null)
            {
                var newsFolder = Path.Combine(_env.WebRootPath, "news-photos");
                if (!Directory.Exists(newsFolder)) Directory.CreateDirectory(newsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(update.NewsPhoto.FileName)}";
                var filePath = Path.Combine(newsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await update.NewsPhoto.CopyToAsync(stream);
                }
                update.PhotoUrl = $"/news-photos/{uniqueFileName}";

            }
                try
            {
                var article = await _newsService.GetByIdAsync(id);
                if (article == null)
                {
                    return NotFound("News article not found");
                }
                article.PhotoUrl = update.PhotoUrl;
                article.Title = update.Title;
                article.Content = update.Content;
                article.UpdatedDate = DateTime.UtcNow;
                await _newsService.UpdateAsync(article);
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNewsArticle(Guid id)
        {
            try
            {
                await _newsService.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }

}
