using GauCorner.Business.Services.ProductServices;
using GauCorner.Data.DTO.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GauCorner.API.Controller
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _donateServices;
        public ProductController(IProductServices donateServices)
        {
            _donateServices = donateServices;
        }

        [HttpPost]
        public async Task<IActionResult> Donate([FromForm] ProductFormReqModel productReqModel)
        {
            var variants = JsonConvert.DeserializeObject<List<ProductVariant>>(productReqModel.VariantsJson);
            var files = Request.Form.Files;

            // Group theo tên file: Images[0], Images[1], ...
            var groupedImages = files
                .Where(f => f.Name.StartsWith("Images["))
                .GroupBy(f => f.Name)
                .ToDictionary(g => g.Key, g => g.ToList());

            for (int i = 0; i < variants.Count; i++)
            {
                var key = $"Images[{i}]";
                if (groupedImages.TryGetValue(key, out var images))
                {
                    variants[i].Image = images;
                }
            }
            var result = await _donateServices.CreateProduct(productReqModel, variants);
            return Ok(result);
        }
    }
}
