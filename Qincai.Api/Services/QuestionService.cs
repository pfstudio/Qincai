﻿using Microsoft.EntityFrameworkCore;
using Qincai.Api.Dtos;
using Qincai.Api.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Qincai.Api.Services
{
    public interface IQuestionService
    {
        Task<Question> GetByIdAsync(Guid questionId);
        Task<Question> CreateAsync(User questioner, CreateQuestion dto);
        IQueryable<Question> GetQuery(Expression<Func<Question, bool>> filter=null);
        Task<Answer> ReplyAsync(Guid questionId, User answerer, ReplyQuestion dto);
        IQueryable<Answer> GetAnswersQuery(Guid questionId, Expression<Func<Answer, bool>> filter=null);
        Task<bool> ExistAsync(Guid questionId);
    }

    public class QuestionService : IQuestionService
    {
        private readonly ApplicationDbContext _context;

        public QuestionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Question> CreateAsync(User questioner, CreateQuestion dto)
        {
            // 创建新问题
            var question = new Question
            {
                // 为了方便测试，Guid由代码直接生成
                // 转入生成环境后，Guid由数据库自主生成
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Content = new Content
                {
                    Text = dto.Content
                },
                QuestionTime = DateTime.Now,
                LastTime = DateTime.Now,
                Questioner = questioner
            };

            // 保存修改
            await _context.AddAsync(question);
            await _context.SaveChangesAsync();

            return question;
        }

        public Task<bool> ExistAsync(Guid questionId)
        {
            return _context.Questions.AnyAsync(q => q.Id == questionId);
        }

        public IQueryable<Answer> GetAnswersQuery(Guid questionId, Expression<Func<Answer, bool>> filter=null)
        {
            var query =  _context.Answers
                .Include(a => a.Answerer)
                .Include(a => a.Question)
                .Include(a => a.RefAnswer)
                .Where(a => a.Question.Id == questionId)
                .OrderByDescending(a => a.AnswerTime)
                .AsNoTracking();

            return filter == null ? query : query.Where(filter);
        }

        public Task<Question> GetByIdAsync(Guid questionId)
        {
            return _context.Questions
                .Include(q => q.Questioner)
                .Where(q => q.Id == questionId)
                .SingleOrDefaultAsync();
        }

        public IQueryable<Question> GetQuery(Expression<Func<Question, bool>> filter=null)
        {
            var query = _context.Questions
                .Include(q => q.Questioner)
                .OrderByDescending(q => q.QuestionTime)
                .AsNoTracking()
                .AsQueryable();

            return filter == null ? query : query.Where(filter);
        }

        public async Task<Answer> ReplyAsync(Guid questionId, User answerer, ReplyQuestion dto)
        {
            Question question = await _context.Questions
                .Include(q => q.Answers)
                .ThenInclude(a => a.Answerer)
                .Include(q => q.Answers)
                .ThenInclude(a => a.RefAnswer)
                .Where(q => q.Id == questionId)
                .SingleAsync();

            // TODO: 暂时对引用问题不存在的问题，作静默处理
            Answer refAnswer = await _context.Answers
                .Where(a => a.Id == dto.RefAnswerId)
                .SingleOrDefaultAsync();

            Answer answer = new Answer
            {
                Id = Guid.NewGuid(),
                Answerer = answerer,
                AnswerTime = DateTime.Now,
                Content = new Content
                {
                    Text = dto.Content
                },
                Question = question,
                RefAnswer = refAnswer
            };
            question.Answers.Add(answer);

            _context.Update(question);
            await _context.SaveChangesAsync();

            return answer;
        }
    }
}
