using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManger.Core.Domain.DTO
{
    public class NewsRequestDTO
    {
        public string Title { get; set; }
        public string? Content { get; set; }
        [NotMapped]
        public IFormFile? NewsPhoto { get; set; }

        public string? PhotoUrl { get; set; }
    }
}
