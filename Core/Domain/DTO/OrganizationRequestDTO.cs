using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EventManger.Core.Domain.Entities
{
    public class OrganizationRequestDTO
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
        public string? College { get; set; }
        public string? ContactNumber { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public IFormFile? Logo { get; set; }
        public string? LogoUrl { get; set; }
    }
}
