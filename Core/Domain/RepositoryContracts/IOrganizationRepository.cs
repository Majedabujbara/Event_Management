using EventManger.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManger.Core.Domain.RepositoryContracts
{
    public interface IOrganizationRepository
    {
        Task<IEnumerable<Organization>> GetAllOrganizationsAsync();
        Task<Organization> GetOrganizationAsync(Guid id);
        Task AddOrganizationAsync(Organization organization);
        Task RemoveOrganizationAsync(Guid id);
        Task EditOrganizationAsync(Organization organization);
    }
}