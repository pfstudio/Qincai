using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qincai.Api.Dtos;
using Qincai.Api.Models;

namespace Qincai.Api.Controllers
{
    /// <summary>
    /// 问题相关API
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public QuestionController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// 创建一个新的问题
        /// </summary>
        /// <param name="model">新问题</param>
        /// <param name="userId">用户Id</param>
        [HttpPost("[Action]")]
        public ActionResult Create([FromBody]CreateQuestion model, [FromHeader]Guid userId)
        {
            // 验证用户身份
            // 终极简化版
            if (userId == null)
            {
                return Unauthorized();
            }

            User questioner = _context.Users.Find(userId);

            // 创建新问题
            var question = new Question
            {
                // 为了方便测试，Guid由代码直接生成
                // 转入生成环境后，Guid由数据库自主生成
                Id = Guid.NewGuid(),
                Title = model.Title,
                Content = new Content
                {
                    Text = model.Content
                },
                QuestionTime = DateTime.Now,
                LastTime = DateTime.Now,
                Questioner = questioner
            };
            // 保存修改
            _context.Add(question);
            _context.SaveChanges();
            // 返回201，以及location uri
            // 不应直接返回任何数据库模型！！！
            // 此处使用AutoMap实现映射
            return CreatedAtRoute("GetQuestion", new { question.Id }, _mapper.Map<QuestionDTO>(question));
        }

        /// <summary>
        /// 问题列表
        /// </summary>
        /// <remarks>后期需要按某一字段排序</remarks>
        /// <param name="page">页数</param>
        /// <param name="pagesize">每页数量</param>
        [HttpGet]
        public ActionResult<PagedResult<QuestionDTO>> List([FromQuery]int page=1, [FromQuery]int pagesize=10)
        {
            var questions = _context.Questions
                .Include(q => q.Questioner)
                .AsNoTracking().AsQueryable()
                // 此处使用ProjectTo拓展实现对IQueryable对象的映射
                .ProjectTo<QuestionDTO>(_mapper.ConfigurationProvider);
            var tmp = questions.ToList();
            if(page < 1 || pagesize > 50)
            {
                return BadRequest("分页错误");
            }
            // 利用PagedResult，实现分页
            // TODO：缺少更高级的分页控制
            return PagedResult<QuestionDTO>.Filter(questions, page, pagesize);
        }

        /// <summary>
        /// 根据Id获取问题
        /// </summary>
        /// <param name="id">问题Id</param>
        //  此处Name是因为CreateAtRoute需要
        [HttpGet("{id}", Name = "GetQuestion")]
        public ActionResult<QuestionDTO> GetById([FromRoute]Guid id)
        {
            var question = _context.Questions
                .Include(q => q.Questioner)
                .Where(q => q.Id == id)
                .FirstOrDefault();
            if (question == null)
            {
                return NotFound();
            }

            return _mapper.Map<QuestionDTO>(question);
        }

        /// <summary>
        /// 问题的回答列表
        /// </summary>
        /// <param name="id">问题Id</param>
        /// <param name="page">页数</param>
        /// <param name="pagesize">每页数量</param>
        [HttpGet("{id}/Answers")]
        public ActionResult<PagedResult<AnswerDTO>> ListAnswersByQuestionId([FromRoute]Guid id, [FromQuery]int page=1, [FromQuery]int pagesize=10)
        {
            if (_context.Questions.Count(q => q.Id == id) == 0)
            {
                return NotFound();
            }
            var answers = _context.Answers
                .Include(a => a.Answerer)
                .Include(a => a.Question)
                .Where(a => a.Question.Id == id)
                .AsNoTracking()
                .ProjectTo<AnswerDTO>(_mapper.ConfigurationProvider);
            if (page < 1 || pagesize > 50)
            {
                return BadRequest("分页错误");
            }

            return PagedResult<AnswerDTO>.Filter(answers, page, pagesize);
        }

        /// <summary>
        /// 回答问题
        /// </summary>
        /// <param name="id">问题Id</param>
        /// <param name="model">新回答</param>
        /// <param name="userId">用户Id</param>
        [HttpPost("{id}/Reply")]
        public ActionResult Reply([FromRoute]Guid id, [FromBody]ReplyQuestion model, [FromHeader]Guid userId)
        {
            if (userId == null)
            {
                return Unauthorized();
            }

            Question question = _context.Questions
                .Include(q => q.Answers)
                .ThenInclude(a => a.Answerer)
                .Where(q => q.Id == id)
                .FirstOrDefault();
            if (question == null)
            {
                return NotFound();
            }
            User answerer = _context.Users.Find(userId);
            Answer answer = new Answer
            {
                Id = Guid.NewGuid(),
                Answerer = answerer,
                AnswerTime = DateTime.Now,
                Content = new Content
                {
                    Text = model.Content
                },
                Question = question
            };
            question.Answers.Add(answer);

            _context.Update(question);
            _context.SaveChanges();

            return CreatedAtRoute("GetQuestion", new { question.Id }, _mapper.Map<AnswerDTO>(answer));
        }
    }
}