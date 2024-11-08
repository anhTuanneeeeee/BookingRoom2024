using System;
using System.Collections.Generic;

namespace BOs.Entity;

public partial class RoomType
{
    public int RoomTypeId { get; set; }

    public string? TypeName { get; set; }

    public string? Description { get; set; }

    public string? Utilities { get; set; }

    public virtual ICollection<Price> Prices { get; set; } = new List<Price>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
