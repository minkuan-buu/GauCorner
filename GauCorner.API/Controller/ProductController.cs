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

            // Lưu từng ảnh theo index
            var savedOptionImages = new List<string>();
            for (int i = 0; i < wrapper.AttributeImage.Length; i++)
            {
                var file = wrapper.AttributeImage[i];
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var path = Path.Combine("./uploads/shop/attributes", fileName);

                await using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);

                savedOptionImages.Add($"/www/wwwroot/api.donate.buubuu.id.vn/GauCorner/GauCorner.API/publish/uploads/shop/attributes/{fileName}");
            }

            // Gán lại image URL vào Option tương ứng
            var parentAttr = productDto.Attribute.FirstOrDefault(a => a.isParent);
            if (parentAttr != null)
            {
                for (int i = 0; i < parentAttr.Options.Count; i++)
                {
                    parentAttr.Options[i].Image = savedOptionImages.ElementAtOrDefault(i);
                }
            }

            var result = await _productServices.CreateProduct(productDto, token);
            return Ok(result);
        }

    }
}
