using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using EventManger.Infrastructure.DBContext;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SearchController(ApplicationDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Ok(new { events = new object[0], news = new object[0], orgs = new object[0] });

        var loweredQuery = query.ToLower();

        var events = await _context.Events
            .Where(e => e.Name.ToLower().Contains(loweredQuery) || e.Description.ToLower().Contains(loweredQuery))
            .Select(e => new { e.EventID, e.Name })
            .ToListAsync();

        var news = await _context.NewsArticles
            .Where(n => n.Title.ToLower().Contains(loweredQuery) || n.Content.ToLower().Contains(loweredQuery))
            .Select(n => new { n.Id, n.Title })
            .ToListAsync();

        var orgs = await _context.Organizations
            .Where(o => o.Name.ToLower().Contains(loweredQuery) || o.Description.ToLower().Contains(loweredQuery))
            .Select(o => new { o.OrganizationID, o.Name })
            .ToListAsync();

        return Ok(new { events, news, orgs });
    }
}
