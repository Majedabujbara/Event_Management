using EventManger.Core.Domain.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManger.Core.Domain.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }
        public virtual ICollection<Event> AttendedEvents { get; set; } = new List<Event>();
        public override string? SecurityStamp { get; set; } = Guid.NewGuid().ToString("D"); 
    }
}
