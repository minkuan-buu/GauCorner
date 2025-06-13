using Microsoft.AspNetCore.Http;

// Model phụ trợ cho deserialize JSON
public class ProductData
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }

    public List<AttributeData> Attributes { get; set; }
    public List<VariantData> Variants { get; set; }
}

public class AttributeData
{
    public string Name { get; set; }
    public bool IsParent { get; set; }
    public Guid? ParentAttributeId { get; set; }
    public List<AttributeValueData> Values { get; set; }
}

public class AttributeValueData
{
    public string Value { get; set; }
    public int? ImageIndex { get; set; } // Tham chiếu ảnh trong AttributeImages
}

public class VariantData
{
    public string SKU { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}
