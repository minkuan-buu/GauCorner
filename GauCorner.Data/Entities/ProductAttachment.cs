using System;
using System.Collections.Generic;

namespace GauCorner.Data.Entities;

public partial class ProductAttachment
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public string Type { get; set; } = null!;

    public string AttachmentUrl { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
