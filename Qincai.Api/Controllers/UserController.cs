using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Qincai.Api.Extensions;
using Qincai.Api.Utils;
using Qincai.Dtos;
using Qincai.Models;
using Qincai.Services;
using System;
using System.Threading.Tasks;

namespace Qincai.Api.Controllers
{
    /// <summary>
    /// 用户相关API
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserService _userService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="mapper">对象映射</param>
        /// <param name="authorizationService">认证服务</param>
        /// <param name="userService">用户服务</param>
        public UserController(IMapper mapper, IAuthorizationService authorizationService, IUserService userService)
        {
            _mapper = mapper;
            _authorizationService = authorizationService;
            _userService = userService;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetById([FromRoute]Guid id)
        {
            User user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// 获取用户自己的信息
        /// </summary>
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> GetMyInfo()
        {
            User user = await _userService.GetByIdAsync(User.GetUserId());

            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateInfo([FromRoute]Guid id, [FromBody]UpdateUserParam dto)
        {
            // 判断是否为当前用户
            var userId = User.GetUserId();
            if (userId != id)
            {
                // 判断是否为管理员
                var authorizationResult = await _authorizationService.AuthorizeAsync(User, AuthorizationPolicies.Admin);
                if (!authorizationResult.Succeeded)
                {
                    return Forbid();
                }
            }

            await _userService.UpdateAsync(dto);

            return NoContent();
        }
    }
}