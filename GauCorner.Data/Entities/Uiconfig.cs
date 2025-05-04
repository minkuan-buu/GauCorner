using System;
using System.Collections.Generic;

namespace GauCorner.Data.Entities;

public partial class Uiconfig
{
    public Guid Id { get; set; }

    public int Index { get; set; }

    public string Name { get; set; } = null!;

    public string BackgroundUrl { get; set; } = null!;

    public string LogoUrl { get; set; } = null!;

    public string ColorTone { get; set; } = null!;

    public string? Description { get; set; }

    public Guid CreatedBy { get; set; }

    public bool IsUsed { get; set; }

    public virtual UserAccount CreatedByNavigation { get; set; } = null!;
}
