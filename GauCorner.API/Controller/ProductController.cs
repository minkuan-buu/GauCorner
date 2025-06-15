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
                var path = Path.Combine("wwwroot/uploads/attributes", fileName);

                await using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);

                savedOptionImages.Add($"/uploads/attributes/{fileName}");
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

            var result = await _productServices.CreateProduct(productDto);
            return Ok(result);
        }

    }
}
