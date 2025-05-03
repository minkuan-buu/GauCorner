using System;
using System.Collections.Generic;

namespace GauCorner.Data.Entities;

public partial class StreamConfig
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid StreamConfigTypeId { get; set; }

    public string Value { get; set; } = null!;

    public virtual StreamConfigType StreamConfigType { get; set; } = null!;

    public virtual UserAccount User { get; set; } = null!;
}
