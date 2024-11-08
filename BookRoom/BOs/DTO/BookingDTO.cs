using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTO
{
    public class BookingDTO
    {
        public int RoomId { get; set; }
        public int SlotId { get; set; }
        public int CustomerId { get; set; } // ID của khách hàng
    }
}
