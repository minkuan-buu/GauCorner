using System;
using System.Collections.Generic;

namespace GauCorner.Data.Entities;

public partial class StreamConfigType
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string AlternativeName { get; set; } = null!;

    public string DefaultValue { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<StreamConfig> StreamConfigs { get; set; } = new List<StreamConfig>();
}
