using System;
using System.Collections.Generic;

namespace GauCorner.Data.Entities;

public partial class ProductSize
{
    public Guid Id { get; set; }

    public Guid VariantId { get; set; }

    public string? Size { get; set; }

    public decimal Price { get; set; }

    public string? Sku { get; set; }

    public int? StockQuantity { get; set; }

    public virtual ProductVariant Variant { get; set; } = null!;
}
