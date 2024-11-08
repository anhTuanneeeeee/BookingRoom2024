using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTO
{
    public class GetRoomByIdDTO
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; } // Add RoomTypeName
        public int BranchId { get; set; }
        public string BranchName { get; set; }   // Add BranchName
        public decimal Price {  get; set; }
    }
}
