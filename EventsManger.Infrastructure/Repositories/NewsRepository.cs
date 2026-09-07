using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;

namespace EventManger.Infrastructure.Repositories
{

    public class NewsRepository : INewsRepository
    {
        private readonly ApplicationDbContext _context;
        public NewsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(NewsArticle article)
        {
            await _context.NewsArticles.AddAsync(article);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var article = await GetByIdAsync(id);
            if (article != null)
            {
                _context.NewsArticles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<NewsArticle>> GetAllAsync()
        {
            return await _context.NewsArticles.OrderByDescending(a => a.CreatedDate).ToListAsync();
        }

        public async Task<NewsArticle> GetByIdAsync(Guid id)
        {
            return await _context.NewsArticles.FindAsync(id);
        }

        public async Task UpdateAsync(NewsArticle article)
        {
            article.UpdatedDate = DateTime.UtcNow;
            _context.NewsArticles.Update(article);
            await _context.SaveChangesAsync();
        }
    }
}
