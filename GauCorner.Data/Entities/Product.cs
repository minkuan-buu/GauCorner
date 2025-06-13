using System;
using System.Collections.Generic;

namespace GauCorner.Data.Entities;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public Guid CategoryId { get; set; }

    public bool Status { get; set; }

    public Guid CreatedBy { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual UserAccount CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<ProductAttachment> ProductAttachments { get; set; } = new List<ProductAttachment>();

    public virtual ICollection<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}
