using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.Identity;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Core.ServicesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManger.Core.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _person;
        public PersonService(IPersonRepository person)
        {
            _person = person;
        }

        /*public async Task AddPersonAsync(ApplicationUser person)
        {
            await _person.AddPersonAsync(person);
        }*/

        public async Task DeletePersonAsync(Guid id)
        {
            await _person.DeletePersonAsync(id);
        }

        public async Task EditPersonAsync(ApplicationUser person)
        {
            await _person.EditPersonAsync(person);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllPersonAsync()
        {
            return await _person.GetAllPersonAsync();
        }

        public async Task<ApplicationUser> GetPersonAsync(Guid id)
        {
            return await _person.GetPersonAsync(id);
        }
    }
}
