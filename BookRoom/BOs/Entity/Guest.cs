using System;
using System.Collections.Generic;

namespace BOs.Entity;

public partial class Guest
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreateUser { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Image { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Role? Role { get; set; }
}
