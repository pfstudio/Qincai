using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qincai.Dtos;
using Qincai.Api.Extensions;
using Qincai.Services;
using System;
using System.Threading.Tasks;

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
        private readonly IMapper _mapper;
        private readonly IAnswerService _answerService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="answerService">回答服务</param>
        /// <param name="mapper">对象映射</param>
        public AnswerController(IAnswerService answerService, IMapper mapper)
        {
            _mapper = mapper;
            _answerService = answerService;
        }

        /// <summary>
        /// 我的回答
        /// </summary>
        /// <param name="dto">分页参数</param>
        [HttpGet("me")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<PagedResult<AnswerWithQuestionDto>>> ListMyAnswer([FromQuery]ListAnswerParam dto)
        {
            Guid userId = User.GetUserId();
            var answers = _answerService
                .GetQuery(a => a.Answerer.Id == userId, dto)
                // 此处使用ProjectTo拓展实现对IQueryable对象的映射
                .ProjectTo<AnswerWithQuestionDto>(_mapper.ConfigurationProvider);

            // 利用PagedResult，实现分页
            return await answers.Paged(dto);
        }
    }
}