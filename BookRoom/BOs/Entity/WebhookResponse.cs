using System;
using System.Collections.Generic;

namespace BOs.Entity;

public partial class WebhookResponse
{
    public string Code { get; set; } = null!;

    public string Desc { get; set; } = null!;

    public string? Data { get; set; }
}
