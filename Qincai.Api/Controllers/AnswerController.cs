using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qincai.Api.Dtos;
using Qincai.Api.Extensions;

namespace Qincai.Api.Controllers
{
    /// <summary>
    /// 回答相关API
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AnswerController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// 我的回答
        /// </summary>
        /// <param name="dto">分页参数</param>
        [HttpGet("me")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<PagedResult<AnswerWithQuestionDto>>> ListMyAnswer([FromQuery]ListAnswer dto)
        {
            Guid userId = User.GetUserId();
            var answers = _context.Answers
                .Include(a => a.Answerer)
                .Include(a => a.Question)
                .Include(a => a.RefAnswer)
                .Where(a => a.Answerer.Id == userId)
                // 此处使用ProjectTo拓展实现对IQueryable对象的映射
                .ProjectTo<AnswerWithQuestionDto>(_mapper.ConfigurationProvider);

            // 利用PagedResult，实现分页
            // TODO：缺少更高级的分页控制
            return await answers.Paged(dto);
        }
    }
}