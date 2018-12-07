using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Qincai.Api.Extensions;
using Qincai.Dtos;
using Qincai.Models;
using Qincai.Services;
using System.Threading.Tasks;

namespace Qincai.Api.Controllers
{
    /// <summary>
    /// 图片管理相关API
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IUserService _userService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="imageService">图片服务</param>
        /// <param name="userService">用户服务</param>
        public ImageController(IImageService imageService, IUserService userService)
        {
            _imageService = imageService;
            _userService = userService;
        }

        /// <summary>
        /// 返回图片上传Token
        /// </summary>
        [HttpGet("UploadToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ImageUploadToken>> GetUploadToken()
        {
            // 获取上传者
            User uploader = await _userService.GetByIdAsync(User.GetUserId());
            // 创建上传Token
            ImageUploadToken token = _imageService.CreateToken(uploader);

            return token;
        }
    }
}