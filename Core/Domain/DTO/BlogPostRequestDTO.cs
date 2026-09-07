using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.Identity;
using Microsoft.AspNetCore.Http;

namespace EventManger.Core.Domain.DTO
{
    public class BlogPostRequestDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        [ForeignKey(nameof(ApplicationUser.Id))]
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime TimePosted { get; set; } = DateTime.Now;
        [NotMapped]
        public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }  
    }
}
