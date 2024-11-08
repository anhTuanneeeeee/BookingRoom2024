using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTO
{
    public class GetRoomByBranchIdDTO
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int RoomTypeId { get; set; }
        public int BranchId { get; set; }
        public decimal Price { get; set; }
    }
}
