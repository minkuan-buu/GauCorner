using System;
using System.Collections.Generic;

namespace GauCorner.Data.Entities;

public partial class ProductType
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
