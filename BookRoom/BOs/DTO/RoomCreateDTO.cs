using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTO
{
    public class RoomCreateDTO
    {
        public string RoomName { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public int RoomTypeId { get; set; }
    }
}
