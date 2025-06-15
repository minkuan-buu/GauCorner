using System.Security.Cryptography.X509Certificates;

public class ProductDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }

    public List<string> ProductImages { get; set; }

    public List<ProductAttributeDto> Attribute { get; set; }
    public List<ProductVariantDto> Variant { get; set; }
}

public class ProductAttributeDto
{
    public string Name { get; set; }
    public bool IsParent { get; set; }
    public List<ProductOptionDto> Options { get; set; }
}

public class ProductOptionDto
{
    public Guid Id { get; set; } // Thêm Id nếu cần thiết
    public string Value { get; set; }
    public string Image { get; set; }  // null nếu không có
}

public class ProductVariantDto
{
    public List<int> AttributeIndex { get; set; } // Index theo từng Option trong Attribute
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string SKU { get; set; }
}
