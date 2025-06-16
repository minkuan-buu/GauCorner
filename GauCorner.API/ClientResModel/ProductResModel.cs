public class ProductFormWrapper
{
    public string Product { get; set; } // JSON string
    public IFormFile[]? AttributeImage { get; set; } // Files theo index
    public IFormFile[] ProductImage { get; set; } // Files theo index
}
