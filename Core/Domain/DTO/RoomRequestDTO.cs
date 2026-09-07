using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManger.Core.Domain.DTO
{
    public class RoomRequestDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Seats { get; set; }
    }
}
