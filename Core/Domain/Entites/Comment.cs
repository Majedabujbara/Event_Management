using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventManger.Core.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventManger.Core.Domain.Entites
{
    public class Comment
    {
        [Key]
        public Guid? Id { get; set; }= Guid.NewGuid();
        public string Content { get; set; }
        public Guid? UserId { get; set; }
        public string? CommentatorName { get; set; }
        public DateTime TimeCommented { get; set; } = DateTime.Now;
        public List<Comment?>? Replies { get; set; } = new List<Comment?>();
        // Navigation properties
        public Guid BlogPostId { get; set; }  // Foreign key
        public BlogPost? BlogPost { get; set; }  // Navigation property
    }
}
