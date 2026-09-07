using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventManger.Core.Domain.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace EventManger.Core.Domain.Entites
{
    public class BlogPost
    {
        [Key]
        public Guid BlogId { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Content { get; set; }
        [ForeignKey(nameof(ApplicationUser.Id))]
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public DateTime TimePosted { get; set; } = DateTime.Now;
        public List<Comment?>? Comments { get; set; } = new List<Comment?>();
        [NotMapped]
        public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }
        public virtual ICollection<ApplicationUser> Likes { get; set; } = new List<ApplicationUser>();
    }
}
