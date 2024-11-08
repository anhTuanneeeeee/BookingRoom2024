using System;
using System.Collections.Generic;

namespace BOs.Entity;

public partial class Room
{
    public int RoomId { get; set; }

    public string? RoomName { get; set; }

    public int? RoomTypeId { get; set; }

    public int? BranchId { get; set; }

    public int? StatusId { get; set; }

    public bool? IsAvailable { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Branch? Branch { get; set; }

    public virtual RoomType? RoomType { get; set; }

    public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();

    public virtual Status? Status { get; set; }
}
