using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManger.Core.Domain.Identity;

namespace EventManger.Core.Domain.Entites
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
