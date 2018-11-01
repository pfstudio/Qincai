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

        [HttpPost("[Action]")]
        public ActionResult Create([FromBody]CreateQuestion model, [FromHeader]Guid userId)
        {
            if (userId == null)
            {
                return Unauthorized();
            }

            User questioner = _context.Users.Find(userId);

            var question = new Question
            {
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

            _context.Add(question);
            _context.SaveChanges();

            return CreatedAtRoute("GetQuestion", new { question.Id }, _mapper.Map<QuestionDTO>(question));
        }

        [HttpGet]
        public ActionResult<PagedResult<QuestionDTO>> List([FromQuery]int page=1, [FromQuery]int pagesize=10)
        {
            var questions = _context.Questions
                .Include(q => q.Questioner)
                .AsNoTracking().AsQueryable()
                .ProjectTo<QuestionDTO>(_mapper.ConfigurationProvider);
            var tmp = questions.ToList();
            if(page < 1 || pagesize > 50)
            {
                return BadRequest("分页错误");
            }
            return PagedResult<QuestionDTO>.Filter(questions, page, pagesize);
        }

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