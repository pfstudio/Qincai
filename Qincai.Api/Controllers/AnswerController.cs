using AutoMapper;
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
        private readonly IAuthorizationService _authorizationService;
        private readonly IAnswerService _answerService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="answerService">回答服务</param>
        /// <param name="mapper">对象映射</param>
        /// <param name="authorizationService">认证服务</param>
        public AnswerController(IAnswerService answerService, IMapper mapper,
            IAuthorizationService authorizationService)
        {
            _mapper = mapper;
            _answerService = answerService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// 我的回答
        /// </summary>
        /// <param name="dto">分页参数</param>
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<AnswerWithQuestionDto>>> ListMyAnswers([FromQuery]ListAnswerParam dto)
        {
            Guid userId = User.GetUserId();
            var answers = _answerService
                .GetQuery(a => a.Answerer.Id == userId, dto)
                // 此处使用ProjectTo拓展实现对IQueryable对象的映射
                .ProjectTo<AnswerWithQuestionDto>(_mapper.ConfigurationProvider);

            // 利用PagedResult，实现分页
            return await answers.Paged(dto);
        }

        /// <summary>
        /// 删除回答
        /// </summary>
        /// <param name="id">回答Id</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAnswer([FromRoute]Guid id)
        {
            // 获取要删除的回答
            Answer answer = await _answerService.GetByIdAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            // 检验权限
            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User, answer, AuthorizationPolicies.Ownered);
            // 授权失败
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }
            // 删除回答
            await _answerService.DeleteAsync(id);

            return NoContent();
        }
    }
}