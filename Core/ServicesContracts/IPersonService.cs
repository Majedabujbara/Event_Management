using EventManger.Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManger.Core.ServicesContracts
{
    public interface IPersonService
    {
        Task<IEnumerable<ApplicationUser>> GetAllPersonAsync();
        Task<ApplicationUser> GetPersonAsync(Guid id);
        //Task AddPersonAsync(ApplicationUser person);
        Task DeletePersonAsync(Guid id);
        Task EditPersonAsync(ApplicationUser person);
    }
}
