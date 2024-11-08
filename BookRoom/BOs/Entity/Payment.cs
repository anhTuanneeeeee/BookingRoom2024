using System;
using System.Collections.Generic;

namespace BOs.Entity;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int BookingId { get; set; }

    public int Id { get; set; }

    public decimal Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public int StatusId { get; set; }

    public string? StatusPayment { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Guest IdNavigation { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
