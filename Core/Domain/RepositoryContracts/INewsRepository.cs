using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;

namespace EventManger.Core.Domain.RepositoryContracts
{
    public interface INewsRepository
    {
        Task<NewsArticle> GetByIdAsync(Guid id);
        Task<IEnumerable<NewsArticle>> GetAllAsync();
        Task AddAsync(NewsArticle article);
        Task UpdateAsync(NewsArticle article);
        Task DeleteAsync(Guid id);
    }
}
