public class ProductDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public List<string> ProductImage { get; set; } // chứa tên "ProductImage[i]"
    public List<AttributeDto> Attribute { get; set; }
    public List<VariantDto> Variant { get; set; }
}

public class AttributeDto
{
    public string Name { get; set; }
    public bool isParent { get; set; }
    public List<AttributeOptionDto> Options { get; set; }
}

public class AttributeOptionDto
{
    public string Value { get; set; }
    public string Image { get; set; } // chứa tên "AttributeImage[i]"
}

public class VariantDto
{
    public List<int> AttributeIndex { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string SKU { get; set; }
}

public class PaginationRequest
{
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = "Name"; // Default sort by Name
    public bool? IsDescending { get; set; } = false;
    public string? Keyword { get; set; }
    public Guid? CategoryId { get; set; }
}
