using GauCorner.Data.DTO.Custom;
using GauCorner.Data.DTO.RequestModel;
using GauCorner.Services.CategoryServices;
using GauCorner.Business.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GauCorner.API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;
        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetInformation()
        {
            try
            {
                var Result = await _categoryServices.GetListCategory();
                return Ok(Result);
            }
            catch (CustomException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryReqModel categoryReq)
        {
            try
            {
                var Result = await _categoryServices.CreateCategory(categoryReq);
                return Ok(Result);
            }
            catch (CustomException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryReqModel categoryReq)
        {
            try
            {
                var Result = await _categoryServices.CreateCategory(categoryReq);
                return Ok(Result);
            }
            catch (CustomException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
