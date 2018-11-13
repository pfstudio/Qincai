using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Qincai.Api.Dtos;
using Qincai.Api.Models;
using Qincai.Api.Services;
using Qincai.Api.Utils;
using Senparc.Weixin;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Senparc.Weixin.WxOpen.Containers;
using System.Threading.Tasks;

namespace Qincai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WxOpenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly WxOpenConfig _wxOpenConfig;
        private readonly JwtConfig _jwtConfig;

        public WxOpenController(IUserService userService, IMapper mapper,
            IOptions<WxOpenConfig> wxOpenConfig, IOptions<JwtConfig> jwtConfig)
        {
            _mapper = mapper;
            _wxOpenConfig = wxOpenConfig.Value;
            _userService = userService;
            _jwtConfig = jwtConfig.Value;
        }

        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="model">登录参数</param>
        [HttpPost("[Action]")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<WxOpenLoginResult>> Login([FromBody]WxOpenLogin model)
        {
            // 根据code从服务器换取session_id, session_key
            var jsonResult = await SnsApi.JsCode2JsonAsync(
                _wxOpenConfig.AppId, _wxOpenConfig.Secret, model.Code);
            if (jsonResult.errcode != ReturnCode.请求成功)
            {
                return WxOpenLoginResult.Fail(jsonResult.errmsg);
            }
            // unionId 暂不使用
            string unionId = jsonResult.unionid ?? "";
            // 新建3rd-Session
            // TODO: 3rd-Session需要处理过期
            // TODO: 是否有自动过期机制
            var sessionBag = SessionContainer.UpdateSession(null, jsonResult.openid, jsonResult.session_key, unionId);
            // 根据openid查找用户
            User user = await _userService.GetByOpenIdAsync(jsonResult.openid);

            if (user == null)
            {
                return WxOpenLoginResult.UnRegister(sessionBag.Key);
            }
            else
            {
                return WxOpenLoginResult.Success(sessionBag.Key, _mapper.Map<UserDto>(user));
            }
        }

        /// <summary>
        /// 微信注册
        /// </summary>
        /// <param name="model">注册参数</param>
        [HttpPost("[Action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserDto>> Register([FromBody]WxOpenRegister model)
        {
            // 解码用户信息
            // TODO: 可能存在解密失败的问题
            var userInfo = Senparc.Weixin.WxOpen.Helpers.EncryptHelper.DecodeUserInfoBySessionId(
                model.SessionId, model.EncryptedData, model.Iv);
            // TODO: 可能需要检查签名和水印
            // 判断用户是否已注册
            // TODO: 封装后应该提供其他查找方式
            if (await _userService.ExistByOpenIdAsync(userInfo.openId))
            {
                ModelState.AddModelError("openId", "该用户已注册!");
                return BadRequest(ModelState);
            }
            // 新建用户
            User user = await _userService.CreateAsync(new CreateUser
            {
                Name = userInfo.nickName,
                AvatarUrl = userInfo.avatarUrl,
                WxOpenId = userInfo.openId,
                Role = "用户"
            });

            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// 微信授权
        /// </summary>
        /// <param name="model">登录态参数</param>
        [HttpPost("[Action]")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<AuthorizeResult>> Authorize([FromBody]WxOpenAuthorize model)
        {
            var session = SessionContainer.GetSession(model.SessionId);
            if (session == null)
            {
                return AuthorizeResult.Fail("sesiion 已过期");
            }
            User user = await _userService.GetByOpenIdAsync(session.OpenId);

            var token = JwtTokenHelper.Create(_jwtConfig, user);

            return AuthorizeResult.Success(token);
        }
    }
}