using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;

namespace EventManger.Core.ServicesContracts
{
    public interface INewsService
    {
        Task<NewsArticle> GetByIdAsync(Guid id);
        Task<IEnumerable<NewsArticle>> GetAllAsync();
        Task<Guid> CreateAsync(NewsRequestDTO news);
        Task UpdateAsync(NewsArticle news);
        Task DeleteAsync(Guid id);
    }
}
