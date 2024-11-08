using System;
using System.Collections.Generic;

namespace BOs.Entity;

public partial class Branch
{
    public int BranchId { get; set; }

    public string? BranchName { get; set; }

    public string? Location { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
