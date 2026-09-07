using EventManger.Core.Domain.Entities;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EventManger.Infrastructure.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ApplicationDbContext _context;

        public OrganizationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddOrganizationAsync(Organization organization)
        {
            if (await _context.Organizations.SingleOrDefaultAsync(x => x.OrganizationID == organization.OrganizationID) != null)
            {
                throw new Exception("Organization ID Already Exists");
            }
            var validationContext = new ValidationContext(organization);
            Validator.ValidateObject(organization, validationContext, validateAllProperties: true);

            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();
        }

        public async Task EditOrganizationAsync(Organization organization)
        {
            var existing = await _context.Organizations.FindAsync(organization.OrganizationID);
            if (existing == null) throw new KeyNotFoundException("Organization not found");

            var validationContext = new ValidationContext(organization);
            Validator.ValidateObject(organization, validationContext, validateAllProperties: true);

            _context.Entry(existing).CurrentValues.SetValues(organization);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Organization>> GetAllOrganizationsAsync()
        {
            return await _context.Organizations.ToListAsync();
        }

        public async Task<Organization> GetOrganizationAsync(Guid id)
        {
            return await _context.Organizations.FindAsync(id)
                ?? throw new KeyNotFoundException("Organization not found");
        }

        public async Task RemoveOrganizationAsync(Guid id)
        {
            var entity = await _context.Organizations.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException("Organization not found");

            _context.Organizations.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
