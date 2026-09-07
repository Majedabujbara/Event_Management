using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManger.Core.Domain.Entities
{
    public class Organization
    {
        [Key]
        public Guid OrganizationID { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
        public string? College { get; set; }
        public string? ContactNumber { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [NotMapped]
        public IFormFile? Logo { get; set; }

        public string? LogoUrl { get; set; } 
    }
}
