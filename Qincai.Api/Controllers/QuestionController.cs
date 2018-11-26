using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qincai.Api.Dtos;
using Qincai.Api.Extensions;
using Qincai.Api.Models;
using Qincai.Api.Services;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Qincai.Api.Controllers
{
    /// <summary>
    /// 问题相关API
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQuestionService _questsionService;
        private readonly IUserService _userService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="mapper">对象映射</param>
        /// <param name="questionService">问题相关的服务</param>
        /// <param name="userService">用户相关的服务</param>
        public QuestionController(IMapper mapper, IQuestionService questionService, IUserService userService)
        {
            _mapper = mapper;
            _questsionService = questionService;
            _userService = userService;
        }

        /// <summary>
        /// 问题列表
        /// </summary>
        /// <remarks>后期需要按某一字段排序</remarks>
        /// <param name="dto">问题列表参数</param>
        [HttpGet]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResult<QuestionDto>>> List([FromQuery]SearchQuestionParam dto)
        {
            Expression<Func<Question, bool>> filter = null;
            if (dto.Search != null)
            {
                // TODO: 忽略大小写
                filter = q => q.Title.Contains(dto.Search);
            }
            var questions = _questsionService.GetQuery(filter, dto)
                // 此处使用ProjectTo拓展实现对IQueryable对象的映射
                .ProjectTo<QuestionDto>(_mapper.ConfigurationProvider);

            // 基于拓展实现分页
            var pagedResult = await questions.Paged(dto);

            return pagedResult;
        }

        /// <summary>
        /// 我的问题
        /// </summary>
        /// <param name="dto">分页参数</param>
        [HttpGet("me")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<PagedResult<QuestionDto>>> ListMyQuestion([FromQuery]ListQuestionParam dto)
        {
            Guid userId = User.GetUserId();
            var questions = _questsionService
                .GetQuery(q => q.Questioner.Id == userId, dto)
                // 此处使用ProjectTo拓展实现对IQueryable对象的映射
                .ProjectTo<QuestionDto>(_mapper.ConfigurationProvider);

            // 基于拓展实现分页
            return await questions.Paged(dto);
        }

        /// <summary>
        /// 根据Id获取问题
        /// </summary>
        /// <param name="id">问题Id</param>
        //  此处Name是因为CreateAtRoute需要
        [HttpGet("{id}", Name = "GetQuestion")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [AllowAnonymous]
        public async Task<ActionResult<QuestionDto>> GetById([FromRoute]Guid id)
        {
            var question = await _questsionService.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            return _mapper.Map<QuestionDto>(question);
        }

        /// <summary>
        /// 问题的回答列表
        /// </summary>
        /// <param name="id">问题Id</param>
        /// <param name="dto">回答列表参数</param>
        [HttpGet("{id}/Answers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResult<AnswerDto>>> ListAnswersByQuestionId(
            [FromRoute]Guid id, [FromQuery]ListAnswerParam dto)
        {
            // 判断问题是否存在
            if (!await _questsionService.ExistAsync(id))
            {
                return NotFound();
            }

            var answers = _questsionService
                .GetAnswersQuery(id, dto)
                .ProjectTo<AnswerDto>(_mapper.ConfigurationProvider);

            return await answers.Paged(dto);
        }

        /// <summary>
        /// 创建一个新的问题
        /// </summary>
        /// <param name="dto">新问题</param>
        [HttpPost("[Action]")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<QuestionDto>> Create([FromBody]CreateQuestionParam dto)
        {
            User questioner = await _userService.GetByIdAsync(User.GetUserId());
            Question question = await _questsionService.CreateAsync(questioner, dto);

            // 返回201，以及location uri
            // 不应直接返回任何数据库模型！！！
            // 此处使用AutoMap实现映射
            return CreatedAtRoute("GetQuestion", new { question.Id }, _mapper.Map<QuestionDto>(question));
        }

        /// <summary>
        /// 回答问题
        /// </summary>
        /// <param name="id">问题Id</param>
        /// <param name="dto">新回答</param>
        [HttpPost("{id}/Reply")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AnswerDto>> Reply([FromRoute]Guid id, [FromBody]ReplyQuestionParam dto)
        {
            // 判断问题是否存在
            if (!await _questsionService.ExistAsync(id))
            {
                return NotFound();
            }

            User answerer = await _userService.GetByIdAsync(User.GetUserId());

            Answer answer = await _questsionService.ReplyAsync(id, answerer, dto);

            return CreatedAtRoute("GetQuestion", new { id }, _mapper.Map<AnswerDto>(answer));
        }
    }
}