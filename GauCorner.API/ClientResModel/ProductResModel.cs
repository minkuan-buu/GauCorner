using Microsoft.AspNetCore.Mvc;

public class ProductCreateModel
{
    // Product data (JSON)
    [FromForm(Name = "productData")]
    public string ProductDataJson { get; set; }

    // Ảnh chính (nhiều ảnh)
    [FromForm(Name = "mainImages")]
    public List<IFormFile> MainImages { get; set; } = new();

    // Ảnh thuộc tính (nhiều ảnh)
    [FromForm(Name = "attributeImages")]
    public List<IFormFile> AttributeImages { get; set; } = new();

    // Ảnh biến thể (nhiều ảnh)
    [FromForm(Name = "variantImages")]
    public List<IFormFile> VariantImages { get; set; } = new();
}