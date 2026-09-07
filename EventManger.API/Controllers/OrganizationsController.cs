using EventManger.Core.Domain.Entities;
using EventManger.Core.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EventManger.API.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        private readonly IWebHostEnvironment _env;

        public OrganizationsController(IOrganizationService organizationService, IWebHostEnvironment env)
        {
            _organizationService = organizationService;
            _env = env;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllOrganizations()
        {
            var organizations = await _organizationService.GetAllOrganizationsAsync();
            return Ok(organizations);
        }

        [HttpGet("{OrganizationId}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetOrganization(Guid OrganizationId)
        {
            var result = await _organizationService.GetOrganizationAsync(OrganizationId);
            if (result == null) return NotFound("Organization not found");
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrganization([FromForm] OrganizationRequestDTO organizationDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (organizationDto.Logo != null)
            {
                var logosFolder = Path.Combine(_env.WebRootPath, "logos");
                if (!Directory.Exists(logosFolder)) Directory.CreateDirectory(logosFolder);

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(organizationDto.Logo.FileName)}";
                var filePath = Path.Combine(logosFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await organizationDto.Logo.CopyToAsync(stream);
                }

                organizationDto.LogoUrl = $"/logos/{uniqueFileName}";
            }

            var created = await _organizationService.AddOrganizationAsync(organizationDto);
            return CreatedAtAction(nameof(GetOrganization), new { OrganizationId = created.OrganizationID }, created);
        }

        [HttpPatch]
        public async Task<ActionResult> EditOrganization(Guid id, [FromForm] OrganizationRequestDTO organizationDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (organizationDto.Logo != null)
            {
                var logosFolder = Path.Combine(_env.WebRootPath, "logos");
                if (!Directory.Exists(logosFolder)) Directory.CreateDirectory(logosFolder);

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(organizationDto.Logo.FileName)}";
                var filePath = Path.Combine(logosFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await organizationDto.Logo.CopyToAsync(stream);
                }

                organizationDto.LogoUrl = $"/logos/{uniqueFileName}";
            }


            await _organizationService.EditOrganizationAsync(id, organizationDto);
            return NoContent();
        }


        [HttpDelete("{OrganizationId}")]
        public async Task<IActionResult> DeleteOrganization(Guid OrganizationId)
        {
            await _organizationService.RemoveOrganizationAsync(OrganizationId);
            return Ok($"Deleted Organization {OrganizationId}");
        }
    }
}
