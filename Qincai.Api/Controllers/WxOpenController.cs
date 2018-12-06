using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Qincai.Dtos;
using Qincai.Models;
using Qincai.Services;
using Senparc.Weixin;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Senparc.Weixin.WxOpen.Containers;
using Senparc.Weixin.WxOpen.Entities;
using Senparc.Weixin.WxOpen.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Qincai.Api.Controllers
{
    /// <summary>
    /// 微信小程序相关API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WxOpenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly WxOpenConfig _wxOpenConfig;
        private readonly JwtConfig _jwtConfig;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="userService">用户服务</param>
        /// <param name="mapper">对象映射</param>
        /// <param name="wxOpenConfig">微信小程序配置</param>
        /// <param name="jwtConfig">JWT配置</param>
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
        public async Task<ActionResult<WxOpenLoginResult>> Login([FromBody]WxOpenLoginParam model)
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
        /// <param name="dto">注册参数</param>
        [HttpPost("[Action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserDto>> Register([FromBody]WxOpenRegisterParam dto)
        {
            // TODO: 检查签名
            // 解码用户信息
            // TODO: 是否存在空引用
            DecodedUserInfo userInfo = null;
            try
            {
                userInfo = EncryptHelper.DecodeUserInfoBySessionId(
                    dto.SessionId, dto.EncryptedData, dto.Iv);
            }
            // 处理解码错误异常
            catch (System.Security.Cryptography.CryptographicException)
            {
                return BadRequest("解码错误");
            }
            // 检验水印
            if (!userInfo.CheckWatermark(_wxOpenConfig.AppId))
            {
                return BadRequest("非法请求");
            }
            // 判断用户是否已注册
            // TODO: 封装后应该提供其他查找方式
            if (await _userService.ExistByOpenIdAsync(userInfo.openId))
            {
                ModelState.AddModelError("openId", "该用户已注册!");
                return BadRequest(ModelState);
            }
            // 新建用户
            User user = await _userService.CreateAsync(
                new CreateUserParam
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
        public async Task<ActionResult<AuthorizeResult>> Authorize([FromBody]WxOpenAuthorizeParam model)
        {
            // 查找session
            var session = SessionContainer.GetSession(model.SessionId);
            if (session == null)
            {
                return AuthorizeResult.Fail("sesiion 已过期");
            }
            // 查找用户
            User user = await _userService.GetByOpenIdAsync(session.OpenId);
            // 创建Token
            var token = CreateJwtToken(_jwtConfig, user);

            return AuthorizeResult.Success(token);
        }

        /// <summary>
        /// 创建Jwt Token
        /// </summary>
        /// <param name="jwtConfig">Jwt 配置参数</param>
        /// <param name="user">授权用户</param>
        /// <returns>Jwt Token</returns>
        private string CreateJwtToken(JwtConfig jwtConfig, User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            // 添加Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var subject = new ClaimsIdentity(claims);
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret));
            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256Signature);
            // 有效期2min
            var token = tokenHandler.CreateJwtSecurityToken(
                jwtConfig.Issuer, jwtConfig.Audience, subject,
                expires: DateTime.Now.AddMinutes(2), issuedAt: DateTime.Now,
                signingCredentials: credentials);

            return tokenHandler.WriteToken(token);
        }
    }
}