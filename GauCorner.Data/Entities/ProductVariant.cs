using System;
using System.Collections.Generic;

namespace GauCorner.Data.Entities;

public partial class ProductVariant
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public string? VariantName { get; set; }

    public string? Color { get; set; }

    public string? Style { get; set; }

    public bool? Status { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();

    public virtual ICollection<ProductVariantAttachment> ProductVariantAttachments { get; set; } = new List<ProductVariantAttachment>();
}
