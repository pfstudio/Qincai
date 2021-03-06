﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Qincai.Api.Extensions;
using Qincai.Api.Utils;
using Qincai.Dtos;
using Qincai.Models;
using Qincai.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IAuthorizationService _authorizationService;
        private readonly IQuestionService _questsionService;
        private readonly IUserService _userService;
        private readonly IImageService _imageService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="mapper">对象映射</param>
        /// <param name="authorizationService">认证服务</param>
        /// <param name="questionService">问题相关的服务</param>
        /// <param name="userService">用户相关的服务</param>
        /// <param name="imageService"></param>
        public QuestionController(IMapper mapper, IAuthorizationService authorizationService,
            IQuestionService questionService, IUserService userService, IImageService imageService)
        {
            _mapper = mapper;
            _authorizationService = authorizationService;
            _questsionService = questionService;
            _userService = userService;
            _imageService = imageService;
        }

        /// <summary>
        /// 问题列表
        /// </summary>
        /// <remarks>后期需要按某一字段排序</remarks>
        /// <param name="dto">问题列表参数</param>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<QuestionDto>>> ListQuestions([FromQuery]SearchQuestionParam dto)
        {
            // 创建搜索问题的过滤器
            var filter = CreateQuestionFilter(dto);
            var questions = _questsionService.GetQuery(filter, dto)
                // 此处使用ProjectTo拓展实现对IQueryable对象的映射
                .ProjectTo<QuestionDto>(_mapper.ConfigurationProvider);

            // 基于拓展实现分页
            return await questions.Paged(dto);
        }

        /// <summary>
        /// 我的问题
        /// </summary>
        /// <param name="dto">分页参数</param>
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<QuestionDto>>> ListMyQuestions([FromQuery]ListQuestionParam dto)
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
        //  此处Name是因为CreateAtRoute需要，Name指路由名称
        [HttpGet("{id}", Name = nameof(GetQuestionById))]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<QuestionDto>> GetQuestionById([FromRoute]Guid id)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<QuestionDto>> CreateQuestion([FromBody]CreateQuestionParam dto)
        {
            User questioner = await _userService.GetByIdAsync(User.GetUserId());
            // 将图片转为绝对路径
            dto.Images = dto.Images.Select(image => _imageService.ConvertToAbsolute(image)).ToList();
            Question question = await _questsionService.CreateAsync(questioner, dto);

            // 返回201，以及location uri
            // 不应直接返回任何数据库模型！！！
            // 此处使用AutoMap实现映射
            return CreatedAtRoute(nameof(GetQuestionById), new { question.Id }, _mapper.Map<QuestionDto>(question));
        }

        /// <summary>
        /// 回答问题
        /// </summary>
        /// <param name="id">问题Id</param>
        /// <param name="dto">新回答</param>
        [HttpPost("{id}/Reply")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AnswerDto>> ReplyQuestion([FromRoute]Guid id, [FromBody]ReplyQuestionParam dto)
        {
            // 判断问题是否存在
            if (!await _questsionService.ExistAsync(id))
            {
                return NotFound();
            }
            User answerer = await _userService.GetByIdAsync(User.GetUserId());
            dto.Images = dto.Images.Select(image => _imageService.ConvertToAbsolute(image)).ToList();
            Answer answer = await _questsionService.ReplyAsync(id, answerer, dto);

            return CreatedAtRoute(nameof(GetQuestionById), new { id }, _mapper.Map<AnswerDto>(answer));
        }

        /// <summary>
        /// 删除问题
        /// </summary>
        /// <param name="id">问题Id</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteQuestion([FromRoute]Guid id)
        {
            // 获取要删除的问题
            Question question = await _questsionService.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            // 检验权限
            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User, question, AuthorizationPolicies.Ownered);
            // 授权失败
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }
            // 删除问题
            await _questsionService.DeleteAsync(id);

            return NoContent();
        }

        /// <summary>
        /// 创建搜索问题时的过滤器
        /// </summary>
        /// <param name="dto">搜索参数</param>
        /// <returns>过滤器对象</returns>
        private Expression<Func<Question, bool>> CreateQuestionFilter(SearchQuestionParam dto)
        {
            Expression<Func<Question, bool>> filter = null;
            List<Expression<Func<Question, bool>>> filters = new List<Expression<Func<Question, bool>>>();
            // 查找关键词
            if (dto.Search != null)
            {
                // TODO: 忽略大小写
                filters.Add(q => q.Title.Contains(dto.Search));
            }
            // 查找类别
            if (dto.Category != null)
            {
                filters.Add(q => q.Category == dto.Category);
            }
            // 查找提问者
            if (dto.UserId != null)
            {
                filters.Add(q => q.Questioner.Id == dto.UserId);
            }
            // 拼接表达式
            if (filters.Count > 0)
            {
                // Reduce
                filter = filters.Aggregate((left, right) =>
                {
                    ParameterExpression param = left.Parameters[0];
                    return Expression.Lambda<Func<Question, bool>>(
                        Expression.AndAlso(left.Body, Expression.Invoke(right, param)), param);
                });
            }

            return filter;
        }
    }
}