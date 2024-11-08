using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTO
{
    public class GetRoomDTO
    {

        public int RoomId { get; set; }

        public int RoomTypeId { get; set; }
        public string RoomName { get; set; }
        public string BranchName { get; set; } // Thêm thuộc tính BranchName
        public string RoomTypeName { get; set; } // Thêm thuộc tính RoomTypeName
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
    }
}
