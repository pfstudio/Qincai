using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Qincai.Api.Dtos;
using Qincai.Api.Models;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Microsoft.Extensions.Configuration;
using Senparc.Weixin;
using Senparc.Weixin.WxOpen.Containers;
using Microsoft.EntityFrameworkCore;

namespace Qincai.Api.Controllers
{
    /// <summary>
    /// 用户相关API
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        private readonly string _wxOpenAppId;
        private readonly string _wxOpenSecret;

        public UserController(ApplicationDbContext context,
            IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _wxOpenAppId = configuration["WxOpen:AppId"];
            _wxOpenSecret = configuration["WxOpen:Secret"];
        }

        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="model">登录参数</param>
        [HttpPost("[Action]")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<LoginResult>> WxLogin([FromBody]WxLogin model)
        {
            // 根据code从服务器换取session_id, session_key
            var jsonResult = await SnsApi.JsCode2JsonAsync(
                _wxOpenAppId, _wxOpenSecret, model.Code);
            if (jsonResult.errcode == ReturnCode.请求成功)
            {
                // unionId 暂不使用
                string unionId = jsonResult.unionid ?? "";
                // 新建3rd-Session
                // TODO: 3rd-Session需要处理过期
                var sessionBag = SessionContainer.UpdateSession(null, jsonResult.openid, jsonResult.session_key, unionId);
                // 根据openid查找用户
                // TODO: 封装后应该提供其他查找方式
                User user = await _context.Users.Where(u => u.WxOpenId == jsonResult.openid).SingleOrDefaultAsync();

                if (user == null)
                {
                    return LoginResult.UnRegister(sessionBag.Key);
                }
                else
                {
                    return LoginResult.Success(sessionBag.Key, _mapper.Map<UserDto>(user));
                }
            }
            else
            {
                return LoginResult.Fail(jsonResult.errmsg);
            }
        }

        /// <summary>
        /// 微信注册
        /// </summary>
        /// <param name="model">注册参数</param>
        [HttpPost("[Action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<RegisterResult>> WxRegister([FromBody]WxRegister model)
        {
            // 解码用户信息
            var userInfo = Senparc.Weixin.WxOpen.Helpers.EncryptHelper.DecodeUserInfoBySessionId(
                model.SessionId, model.EncryptedData, model.Iv);
            // TODO: 可能需要检查签名和水印
            // 判断用户是否已注册
            // TODO: 封装后应该提供其他查找方式
            if (await _context.Users.CountAsync(u => u.WxOpenId == userInfo.openId) > 0)
            {
                ModelState.AddModelError("openId", "该用户已注册!");
                return BadRequest(ModelState);
            }
            // 新建用户
            User user = new User
            {
                Id = Guid.NewGuid(),
                Name = userInfo.nickName,
                AvatarUrl = userInfo.avatarUrl,
                WxOpenId = userInfo.openId
            };

            // TODO: 数据库异常处理
            _context.Add(user);
            await _context.SaveChangesAsync();

            return RegisterResult.Create(_mapper.Map<UserDto>(user));
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        [HttpGet]
        public IEnumerable<UserDto> List()
        {
            return _context.Users.ProjectTo<UserDto>(_mapper.ConfigurationProvider);
        }

        /// <summary>
        /// 随机返回一个用户(仅供测试使用)
        /// </summary>
        [HttpGet("Random")]
        public UserDto Random()
        {
            User user = _context.Users.OrderBy(x => Guid.NewGuid()).First();
            return _mapper.Map<UserDto>(user);
        }
    }
}