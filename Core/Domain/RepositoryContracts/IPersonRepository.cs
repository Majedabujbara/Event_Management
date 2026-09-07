using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManger.Core.Domain.RepositoryContracts
{
    public interface IPersonRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllPersonAsync();
        Task<ApplicationUser> GetPersonAsync(Guid id);
        //Task AddPersonAsync(ApplicationUser person);
        Task DeletePersonAsync(Guid id);
        Task EditPersonAsync(ApplicationUser person);
    }
}
