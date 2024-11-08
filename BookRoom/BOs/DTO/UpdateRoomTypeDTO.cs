using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTO
{
    public class UpdateRoomTypeDTO
    {
        public string TypeName { get; set; }
        public string? Description { get; set; }
        
        public string? Utilities { get; set; }
    }
}
