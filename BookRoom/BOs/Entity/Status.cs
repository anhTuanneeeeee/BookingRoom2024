using System;
using System.Collections.Generic;

namespace BOs.Entity;

public partial class Status
{
    public int StatusId { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<SlotBooking> SlotBookings { get; set; } = new List<SlotBooking>();

    public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
}
