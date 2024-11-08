using System;
using System.Collections.Generic;

namespace BOs.Entity;

public partial class PaymentResponse
{
    public string Code { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Data { get; set; }

    public string Signature { get; set; } = null!;
}
