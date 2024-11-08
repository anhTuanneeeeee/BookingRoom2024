using System;
using System.Collections.Generic;

namespace BOs.Entity;

public partial class Slot
{
    public int SlotId { get; set; }

    public int RoomId { get; set; }

    public string? StartTime { get; set; }

    public string? EndTime { get; set; }

    public int? StatusId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Room Room { get; set; } = null!;

    public virtual ICollection<SlotBooking> SlotBookings { get; set; } = new List<SlotBooking>();

    public virtual Status? Status { get; set; }
}
