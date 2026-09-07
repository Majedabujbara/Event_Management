using EventManger.Core.Domain.Identity;
using EventManger.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManger.Core.Domain.Entites
{
    public class Event
    {
        public Guid EventID { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        //Connected to a room
        [ForeignKey("RoomID")]
        public Guid? RoomID { get; set; } // Foreign key
        [ForeignKey("OrganizationID")]
        public Guid? OrganizationID { get; set; } // Foreign key
        public virtual ICollection<ApplicationUser>? Attendees { get; set; } = new List<ApplicationUser>();
        //status of the Event
        public EventStatus Status { get; set; }= EventStatus.Scheduled;

        [NotMapped]
        public IFormFile? EventPhoto { get; set; }
        
        public string? PhotoUrl { get; set; }
        public int Views { get; set; } = 0 ;

    }
}
