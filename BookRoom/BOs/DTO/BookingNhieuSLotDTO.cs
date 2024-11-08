using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTO
{
    public class BookingNhieuSLotDTO
    {
        public int RoomId { get; set; }
        public List<int> SlotIds { get; set; } = new List<int>(); // Danh sách các slot ID cần book
        public int CustomerId { get; set; } // ID của khách hàng
    }
}
