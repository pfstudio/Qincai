using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Qincai.Api.Dtos;
using Qincai.Api.Models;
using Qincai.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private readonly WxOpenConfig _wxOpenConfig;
        private readonly JwtConfig _jwtConfig;

        public UserController(ApplicationDbContext context,
            IMapper mapper, IOptions<WxOpenConfig> wxOpenConfig,
            IOptions<JwtConfig> jwtConfig)
        {
            _context = context;
            _mapper = mapper;
            _wxOpenConfig = wxOpenConfig.Value;
            _jwtConfig = jwtConfig.Value;
        }

        /// <summary>
        /// 用户列表（测试用）
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