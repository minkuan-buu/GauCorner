using GauCorner.Business.Services.ProductServices;
using GauCorner.Data.DTO.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace GauCorner.API.Controller
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;
        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductFormWrapper wrapper)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var productDto = JsonSerializer.Deserialize<ProductDto>(wrapper.Product, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Lưu ảnh AttributeImage
            var savedOptionImages = new List<string>();
            for (int i = 0; i < wrapper.AttributeImage.Length; i++)
            {
                var file = wrapper.AttributeImage[i];
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var path = Path.Combine("./uploads/shop/attributes", fileName);

                // Tạo thư mục nếu chưa tồn tại
                var folder = Path.GetDirectoryName(path);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                await using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);

                savedOptionImages.Add($"/www/wwwroot/api.donate.buubuu.id.vn/GauCorner/GauCorner.API/publish/uploads/shop/attributes/{fileName}");
            }
            // Gán lại image URL vào Option tương ứng (cho parent attribute)
            var parentAttr = productDto.Attribute.FirstOrDefault(a => a.isParent);
            if (parentAttr != null)
            {
                for (int i = 0; i < parentAttr.Options.Count; i++)
                {
                    parentAttr.Options[i].Image = savedOptionImages.ElementAtOrDefault(i);
                }
            }

            // === XỬ LÝ ẢNH SẢN PHẨM ===
            var savedProductImages = new List<string>();
            if (wrapper.ProductImage != null && wrapper.ProductImage.Length > 0)
            {
                foreach (var file in wrapper.ProductImage)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var path = Path.Combine("./uploads/shop/products", fileName);

                    // Tạo thư mục nếu chưa tồn tại
                    var folder = Path.GetDirectoryName(path);
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    await using var stream = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(stream);

                    savedProductImages.Add($"/www/wwwroot/api.donate.buubuu.id.vn/GauCorner/GauCorner.API/publish/uploads/shop/products/{fileName}");
                }
            }

            // Gán danh sách ảnh sản phẩm vào ProductDto (giả sử có thuộc tính Images)
            productDto.ProductImage = savedProductImages;

            var result = await _productServices.CreateProduct(productDto, token);
            return Ok(result);
        }
    }
}
