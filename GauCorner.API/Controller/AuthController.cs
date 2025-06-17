using GauCorner.Data.DTO.RequestModel;
using GauCorner.Business.Services.UserServices;
using GauCorner.Data.DTO.Custom;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
            Response.Cookies.Append("DeviceId", result.Response.Data.Auth.DeviceId.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMonths(6)
            });

            Response.Cookies.Append("RefreshToken", result.Response.Data.Auth.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMonths(6)
            });

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var DeviceId = Request.Cookies["DeviceId"];
            if (DeviceId == null)
            {
                throw new CustomException("DeviceId cookie is missing.");
            }
            var result = await _userServices.Logout(Guid.Parse(DeviceId));

            Response.Cookies.Delete("DeviceId", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
            Response.Cookies.Delete("RefreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
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
