using EventManger.Core.Domain.Entities;
using EventManger.Core.Domain.RepositoryContracts;
using EventManger.Core.ServicesContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManger.Core.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<Organization> AddOrganizationAsync(OrganizationRequestDTO organizationDto)
        {
            Organization organization = new Organization
            {
                Name = organizationDto.Name,
                Description = organizationDto.Description,
                College = organizationDto.College,
                ContactNumber = organizationDto.ContactNumber,
                Email = organizationDto.Email,
                LogoUrl = organizationDto.LogoUrl
            };
            await _organizationRepository.AddOrganizationAsync(organization);
            return organization;
        }

        public async Task EditOrganizationAsync(Guid id, OrganizationRequestDTO organizationDto)
        {
            Organization org = await _organizationRepository.GetOrganizationAsync(id);
            if (org == null)
            {
                throw new Exception("Given organization doens't exist");
            }
            try
            {
                org.Name = organizationDto.Name;
                org.Description = organizationDto.Description;
                org.College = organizationDto.College;
                org.ContactNumber = organizationDto.ContactNumber;
                org.Email = organizationDto.Email;
                org.LogoUrl = organizationDto.LogoUrl;
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException($"Id : {id} doesn't exist");
            }
            catch (Exception ex)
            {
                throw new Exception("Error in updating the organization");
            }
            await _organizationRepository.EditOrganizationAsync(org);
        }

        public async Task<IEnumerable<Organization>> GetAllOrganizationsAsync()
        {
            return await _organizationRepository.GetAllOrganizationsAsync();
        }

        public async Task<Organization> GetOrganizationAsync(Guid id)
        {
            return await _organizationRepository.GetOrganizationAsync(id);
        }

        public async Task RemoveOrganizationAsync(Guid id)
        {
            await _organizationRepository.RemoveOrganizationAsync(id);
        }
    }
}
