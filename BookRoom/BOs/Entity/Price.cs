using System;
using System.Collections.Generic;

namespace BOs.Entity;

public partial class Price
{
    public int Id { get; set; }

    public string? Price1 { get; set; }

    public int? RoomTypeId { get; set; }

    public string? DayOfWeek { get; set; }

    public virtual RoomType? RoomType { get; set; }
}
