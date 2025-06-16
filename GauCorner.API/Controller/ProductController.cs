using GauCorner.Business.Services.ProductServices;
using GauCorner.Data.DTO.RequestModel;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{productId}/shop/{slug}")]
        public async Task<IActionResult> GetProductDetail(Guid productId, string slug)
        {
            var result = await _productServices.GetProductDetail(productId, slug);
            return Ok(result);
        }

        [HttpGet("all/shop/{slug}")]
        public async Task<IActionResult> GetProducts([FromQuery] PaginationRequest request, string slug)
        {
            var result = await _productServices.GetAllProducts(request, slug);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "GauCornerAuthentication")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductFormWrapper wrapper)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var productDto = JsonSerializer.Deserialize<ProductDto>(wrapper.Product, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Lưu ảnh AttributeImage
            var savedOptionImages = new List<string>();
            if (wrapper.AttributeImage != null || wrapper.AttributeImage?.Length != 0)
            {
                for (int i = 0; i < wrapper.AttributeImage?.Length; i++)
                {
                    var file = wrapper.AttributeImage[i];
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var path = Path.Combine("/www/wwwroot/cdn.donate.buubuu.id.vn/uploads/shop/attributes", fileName);

                    // Tạo thư mục nếu chưa tồn tại
                    var folder = Path.GetDirectoryName(path);
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    await using var stream = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(stream);

                    savedOptionImages.Add($"https://cdn.donate.buubuu.id.vn/uploads/shop/attributes/{fileName}");
                }
            }
            // Gán lại image URL vào Option tương ứng (cho parent attribute)
            var parentAttr = productDto.Attribute.FirstOrDefault(a => a.isParent);
            if (parentAttr != null && savedOptionImages.Count > 0)
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
                    var path = Path.Combine("/www/wwwroot/cdn.donate.buubuu.id.vn/uploads/shop/products/", fileName);

                    // Tạo thư mục nếu chưa tồn tại
                    var folder = Path.GetDirectoryName(path);
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    await using var stream = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(stream);

                    savedProductImages.Add($"https://cdn.donate.buubuu.id.vn/uploads/shop/products/{fileName}");
                }
            }

            // Gán danh sách ảnh sản phẩm vào ProductDto (giả sử có thuộc tính Images)
            productDto.ProductImage = savedProductImages;

            var result = await _productServices.CreateProduct(productDto, token);
            return Ok(result);
        }
    }
}
