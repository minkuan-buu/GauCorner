using Microsoft.AspNetCore.Http;

public class ProductReqModel
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public Guid ProductTypeId { get; set; }
    public List<ProductVariant>? Variants { get; set; }
}

public class ProductFormReqModel
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public Guid ProductTypeId { get; set; }

    public string VariantsJson { get; set; } = null!;
}

public class ProductVariant
{
    public string VariantName { get; set; } = null!;
    public string? Color { get; set; }
    public string? Style { get; set; }
    public List<IFormFile> Image { get; set; } = null!;
    public List<ProductSize> Sizes { get; set; } = null!;
}

public class ProductSize
{
    public string? Size { get; set; }
    public decimal Price { get; set; }
    public string SKU { get; set; } = null!;
    public int StockQuantity { get; set; }
}