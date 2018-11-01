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

        public UserController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        [HttpGet]
        public IEnumerable<UserDTO> List()
        {
            return _context.Users.ProjectTo<UserDTO>(_mapper.ConfigurationProvider);
        }

        /// <summary>
        /// 随机返回一个用户(仅供测试使用)
        /// </summary>
        [HttpGet("Random")]
        public UserDTO Random()
        {
            User user = _context.Users.OrderBy(x => Guid.NewGuid()).First();
            return _mapper.Map<UserDTO>(user);
        }
    }
}