using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Core.ServicesContracts;
using Ganss.Xss;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventManger.Core.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IHtmlSanitizer _htmlSanitizer;
        public NewsService(INewsRepository newsRepository,IHtmlSanitizer htmlSanitizer)
        {
            _newsRepository = newsRepository;
            _htmlSanitizer = htmlSanitizer;
        }
        public async Task<Guid> CreateAsync(NewsRequestDTO news)
        {
            var sanitizedContent = _htmlSanitizer.Sanitize(news.Content);

            var article = new NewsArticle
            {
                Title = news.Title,
                Content = sanitizedContent,
                PhotoUrl = news.PhotoUrl
            };

            await _newsRepository.AddAsync(article);
            return article.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _newsRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<NewsArticle>> GetAllAsync()
        {
            return await _newsRepository.GetAllAsync();
        }

        public async Task<NewsArticle> GetByIdAsync(Guid id)
        {
            var article = await _newsRepository.GetByIdAsync(id);
            if (article == null)
            {
                throw new ArgumentNullException($"News article with ID {id} not found");
            }
            return article;
        }

        public async Task UpdateAsync(NewsArticle news)
        {
            var existingArticle = await _newsRepository.GetByIdAsync(news.Id);
            if (existingArticle == null)
            {
                throw new ArgumentNullException($"News article with ID {news.Id} not found");
            }

            var sanitizedContent = _htmlSanitizer.Sanitize(news.Content);

            existingArticle.Title = news.Title;
            existingArticle.Content = sanitizedContent;
            existingArticle.UpdatedDate = DateTime.UtcNow;

            await _newsRepository.UpdateAsync(existingArticle);
        }
    }
}
