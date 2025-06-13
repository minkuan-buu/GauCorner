using System;
using System.Collections.Generic;

namespace GauCorner.Data.Entities;

public partial class ProductAttribute
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsParent { get; set; }

    public Guid? ParentAttributeId { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<AttributeValue> AttributeValues { get; set; } = new List<AttributeValue>();

    public virtual ICollection<ProductAttribute> InverseParentAttribute { get; set; } = new List<ProductAttribute>();

    public virtual ProductAttribute? ParentAttribute { get; set; }

    public virtual Product Product { get; set; } = null!;
}
