using GauCorner.Data.DTO.RequestModel;
using GauCorner.Business.Services.UserServices;
using GauCorner.Data.DTO.Custom;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GauCorner.Business.Services.DonateServices;

namespace GauCorner.API.Controller
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IDonateServices _donateServices;
        public AdminController(IDonateServices donateServices)
        {
            _donateServices = donateServices;
        }

        [HttpGet("donate/config/list")]
        [Authorize(AuthenticationSchemes = "GauCornerAuthentication")]
        public async Task<IActionResult> GetDonateConfigList()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var result = await _donateServices.GetConfigLabel(token);
            return Ok(result);
        }

        [HttpGet("donate/config/{configId}")]
        [Authorize(AuthenticationSchemes = "GauCornerAuthentication")]
        public async Task<IActionResult> GetDonateConfig(Guid configId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var result = await _donateServices.GetConfigById(configId, token);
            return Ok(result);
        }
    }
}
