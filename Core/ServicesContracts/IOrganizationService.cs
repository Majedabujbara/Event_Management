using EventManger.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManger.Core.ServicesContracts
{
    public interface IOrganizationService
    {
        Task<IEnumerable<Organization>> GetAllOrganizationsAsync();
        Task<Organization> GetOrganizationAsync(Guid id);
        Task<Organization> AddOrganizationAsync(OrganizationRequestDTO organizationDto);
        Task RemoveOrganizationAsync(Guid id);
        Task EditOrganizationAsync(Guid id, OrganizationRequestDTO organizationDto);
    }
}