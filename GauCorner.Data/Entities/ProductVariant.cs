using System;
using System.Collections.Generic;

namespace GauCorner.Data.Entities;

public partial class ProductVariant
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public string Sku { get; set; } = null!;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<AttributeValue> Values { get; set; } = new List<AttributeValue>();
    public ICollection<VariantAttributeValue> VariantAttributeValues { get; set; }
}
