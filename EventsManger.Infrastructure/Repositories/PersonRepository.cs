using EventManger.Core.Domain.DTO;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.Identity;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Infrastructure.DBContext;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManger.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public PersonRepository(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        /*public async Task AddPersonAsync(ApplicationUser person)
        {
            throw new NotImplementedException();
        }*/

        public async Task DeletePersonAsync(Guid id)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(Convert.ToString(id));
            if (user == null)
            {
                return ;
            }
            await _userManager.DeleteAsync(user);
        }

        public async Task EditPersonAsync(ApplicationUser person)
        {
            IdentityResult result = await _userManager.UpdateAsync(person);
            if(!result.Succeeded)
            {
                string error = string.Join(" | ",result.Errors.Select(x => x.Description));
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllPersonAsync()
        {   
            return _context.Users;
        }

        public async Task<ApplicationUser> GetPersonAsync(Guid id)
        {
            ApplicationUser? user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == id);
            if(user == null)
            {
                throw new ArgumentException("ID Doesn't Exist");
            }
            return user;
        }
    }
}
