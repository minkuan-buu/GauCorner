using System;
using System.Collections.Generic;

namespace GauCorner.Data.Entities;

public partial class ProductVariantAttachment
{
    public Guid Id { get; set; }

    public Guid ProductVariantId { get; set; }

    public string AttachmentUrl { get; set; } = null!;

    public bool Status { get; set; }

    public virtual ProductVariant ProductVariant { get; set; } = null!;
}
