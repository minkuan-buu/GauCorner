using System;
using System.Collections.Generic;

namespace GauCorner.Data.Entities;

public partial class AttributeValue
{
    public Guid Id { get; set; }

    public Guid AttributeId { get; set; }

    public string Value { get; set; } = null!;

    public string? Image { get; set; }

    public virtual ProductAttribute Attribute { get; set; } = null!;

    public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();

    public ICollection<VariantAttributeValue> VariantAttributeValues { get; set; }
}
