﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTO
{
    public class CreateRoomDTO
    {
        public string RoomName { get; set; }
        public int? RoomTypeId { get; set; }
        public int? BranchId { get; set; }
        
        public bool? IsAvailable { get; set; }
        public decimal? Price { get; set; }
    }
}
