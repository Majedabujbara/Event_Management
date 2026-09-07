using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManger.Core.Domain.Entites
{
    public class NewsArticle
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Content { get; set; } // Will store HTML formatted content
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        [NotMapped]
        public IFormFile? NewsPhoto { get; set; }

        public string? PhotoUrl { get; set; }
    }
}
