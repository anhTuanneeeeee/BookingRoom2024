﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTO
{
    public class CreateBookingDTO

    {
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
        public List<int> SlotIds { get; set; }

    }
}