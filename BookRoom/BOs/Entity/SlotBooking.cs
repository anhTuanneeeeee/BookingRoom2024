using System;
using System.Collections.Generic;

namespace BOs.Entity;

public partial class SlotBooking
{
    public int SlotBookingId { get; set; }

    public int BookingId { get; set; }

    public int SlotId { get; set; }

    public int StatusId { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Slot Slot { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
