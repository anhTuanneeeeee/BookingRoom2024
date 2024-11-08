using System;
using System.Collections.Generic;

namespace BOs.Entity;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? Id { get; set; }

    public int? RoomId { get; set; }

    public DateTime BookingDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public decimal? TotalFee { get; set; }

    public int? SlotId { get; set; }

    public virtual Guest? IdNavigation { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Room? Room { get; set; }

    public virtual Slot? Slot { get; set; }

    public virtual ICollection<SlotBooking> SlotBookings { get; set; } = new List<SlotBooking>();
}
