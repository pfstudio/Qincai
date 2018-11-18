using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qincai.Api.Dtos;
using Qincai.Api.Extensions;
using Qincai.Api.Models;
using Qincai.Api.Services;
using System.Threading.Tasks;

namespace Qincai.Api.Controllers
{
    /// <summary>
    /// 图片管理相关API
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IUserService _userService;

        public ImageController(IImageService imageService, IUserService userService)
        {
            _imageService = imageService;
            _userService = userService;
        }

        /// <summary>
        /// 返回图片上传Token
        /// </summary>
        [HttpGet("UploadToken")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ImageToken>> GetUploadToken()
        {
            User uploader = await _userService.GetByIdAsync(User.GetUserId());

            ImageToken token = _imageService.CreateToken(uploader);

            return token;
        }
    }
}