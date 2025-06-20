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
        private readonly IImageService _imageService;
        public AdminController(IDonateServices donateServices, IImageService imageService)
        {
            _donateServices = donateServices;
            _imageService = imageService;
        }

        [HttpGet("donate/config/list")]
        // [Authorize(AuthenticationSchemes = "GauCornerAuthentication")]
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

        [HttpPost("donate/config")]
        [Authorize(AuthenticationSchemes = "GauCornerAuthentication")]
        public async Task<IActionResult> CreateDonateConfig([FromForm] ConfigWrapper request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            List<IFormFile> logo = new List<IFormFile>();
            if (request.LogoImage != null && request.LogoImage.Length > 0)
            {
                logo.Add(request.LogoImage);
            }
            List<IFormFile> background = new List<IFormFile>();
            if (request.BackgroundImage != null && request.BackgroundImage.Length > 0)
            {
                background.Add(request.BackgroundImage);
            }
            var logoUrl = await _imageService.SaveUploadedFiles(logo.ToArray(), "/www/wwwroot/cdn.donate.buubuu.id.vn/uploads/donate/page", "https://cdn.donate.buubuu.id.vn/uploads/donate/page", "logo", token);
            var backgroundUrl = await _imageService.SaveUploadedFiles(background.ToArray(), "/www/wwwroot/cdn.donate.buubuu.id.vn/uploads/donate/page", "https://cdn.donate.buubuu.id.vn/uploads/donate/page", "background", token);
            var requestDto = new ConfigDto
            {
                Name = request.ConfigName,
                Description = request.Description,
                LogoImage = logoUrl.Count > 0 ? logoUrl[0]! : null,
                BackgroundImage = backgroundUrl.Count > 0 ? backgroundUrl[0]! : null,
                ColorTone = request.ColorTone
            };
            var result = await _donateServices.CreateConfig(requestDto, token);
            return Ok(result);
        }

        [HttpPut("donate/config/{configId}")]
        [Authorize(AuthenticationSchemes = "GauCornerAuthentication")]
        public async Task<IActionResult> CreateDonateConfig(Guid configId, [FromForm] ConfigWrapper request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            // Xử lý xóa ảnh cũ nếu có
            var oldConfigImage = await _donateServices.GetConfigImage(configId, token);
            if (oldConfigImage != null)
            {
                var logoImages = new List<string>();
                var backgroundImages = new List<string>();
                if (!string.IsNullOrEmpty(oldConfigImage.LogoUrl))
                {
                    logoImages.Add(oldConfigImage.LogoUrl);
                }
                if (!string.IsNullOrEmpty(oldConfigImage.BackgroundUrl))
                {
                    backgroundImages.Add(oldConfigImage.BackgroundUrl);
                }
                await _imageService.DeleteImages(logoImages, "/www/wwwroot/cdn.donate.buubuu.id.vn/uploads/donate/page", "logo", token);
                await _imageService.DeleteImages(backgroundImages, "/www/wwwroot/cdn.donate.buubuu.id.vn/uploads/donate/page", "background", token);
            }
            List<IFormFile> logo = new List<IFormFile>();
            if (request.LogoImage != null && request.LogoImage.Length > 0)
            {
                logo.Add(request.LogoImage);
            }
            List<IFormFile> background = new List<IFormFile>();
            if (request.BackgroundImage != null && request.BackgroundImage.Length > 0)
            {
                background.Add(request.BackgroundImage);
            }
            var logoUrl = await _imageService.SaveUploadedFiles(logo.ToArray(), "/www/wwwroot/cdn.donate.buubuu.id.vn/uploads/donate", "https://cdn.donate.buubuu.id.vn/uploads/donate/page", "logo", token);
            var backgroundUrl = await _imageService.SaveUploadedFiles(background.ToArray(), "/www/wwwroot/cdn.donate.buubuu.id.vn/uploads/donate", "https://cdn.donate.buubuu.id.vn/uploads/donate/page", "background", token);
            var requestDto = new ConfigDto
            {
                Name = request.ConfigName,
                Description = request.Description,
                LogoImage = logoUrl.Count > 0 ? logoUrl[0]! : null,
                BackgroundImage = backgroundUrl.Count > 0 ? backgroundUrl[0]! : null,
                ColorTone = request.ColorTone
            };
            var result = await _donateServices.UpdateConfig(configId, requestDto, token);
            return Ok(result);
        }
    }
}
