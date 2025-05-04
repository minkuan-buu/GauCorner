using GauCorner.Business.Services.DonateServices;
using GauCorner.Data.DTO.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GauCorner.API.Controller
{
    [Route("api/donate")]
    [ApiController]
    public class DonateController : ControllerBase
    {
        private readonly IDonateServices _donateServices;
        public DonateController(IDonateServices donateServices)
        {
            _donateServices = donateServices;
        }

        [HttpPost("{userPath}")]
        public async Task<IActionResult> Donate([FromBody] DonateReqModel donateReqModel, string userPath)
        {
            var result = await _donateServices.CreateDonate(donateReqModel, userPath);
            return Ok(result);
        }

        [HttpGet("page/{userPath}")]
        public async Task<IActionResult> GetDonatePage(string userPath)
        {
            var result = await _donateServices.GetDonatePage(userPath);
            return Ok(result);
        }

        [HttpPost("check")]
        public async Task<IActionResult> HandleCheckTransaction([FromBody] string apptransid)
        {
            var result = await _donateServices.HandleCheckTransaction(apptransid);
            return Ok(result);
        }
    }
}
