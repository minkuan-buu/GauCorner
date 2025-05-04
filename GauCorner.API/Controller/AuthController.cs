using GauCorner.Data.DTO.RequestModel;
using GauCorner.Business.Services.UserServices;
using GauCorner.Data.DTO.Custom;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GauCorner.API.Controller
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public AuthController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel userLoginModel)
        {
            var result = await _userServices.Login(userLoginModel);
            Response.Cookies.Append("DeviceId", result.Response.DeviceId.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMonths(6)
            });

            Response.Cookies.Append("RefreshToken", result.Response.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMonths(6)
            });

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel userRegisterModel)
        {
            var result = await _userServices.Register(userRegisterModel);
            return Ok(result);
        }

    }
}
