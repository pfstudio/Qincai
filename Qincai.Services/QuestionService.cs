using Microsoft.EntityFrameworkCore;
using Qincai.Dtos;
using Qincai.Models;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Qincai.Services
{
    /// <summary>
    /// 问题相关服务接口
    /// </summary>
    public interface IQuestionService
    {
        /// <summary>
        /// 根据Id获取问题
        /// </summary>
        /// <param name="questionId">问题Id</param>
        /// <returns>问题实体</returns>
        Task<Question> GetByIdAsync(Guid questionId);
        /// <summary>
        /// 查找问题
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="sorted">排序参数</param>
        /// <returns>问题的可查询对象</returns>
        IQueryable<Question> GetQuery(Expression<Func<Question, bool>> filter = null, ISortedParam sorted = null);
        /// <summary>
        /// 查找对应问题的回答
        /// </summary>
        /// <param name="questionId">问题Id</param>
        /// <param name="sorted">排序参数</param>
        /// <returns>回答的可查询对象</returns>
        IQueryable<Answer> GetAnswersQuery(Guid questionId, ISortedParam sorted = null);
        /// <summary>
        /// 根据Id判断问题是否存在
        /// </summary>
        /// <param name="questionId">问题Id</param>
        /// <returns>问题是否存在</returns>
        Task<bool> ExistAsync(Guid questionId);
        /// <summary>
        /// 创建问题
        /// </summary>
        /// <param name="questioner">提问者</param>
        /// <param name="dto">创建问题参数</param>
        /// <returns>创建的问题</returns>
        Task<Question> CreateAsync(User questioner, CreateQuestionParam dto);
        /// <summary>
        /// 回答问题
        /// </summary>
        /// <param name="questionId">问题Id</param>
        /// <param name="answerer">回答者</param>
        /// <param name="dto">回答问题参数</param>
        /// <returns>问题的回答</returns>
        Task<Answer> ReplyAsync(Guid questionId, User answerer, ReplyQuestionParam dto);
        /// <summary>
        /// 软删除问题
        /// </summary>
        /// <param name="questionId">问题Id</param>
        Task DeleteAsync(Guid questionId);
    }

    /// <summary>
    /// 问题服务
    /// </summary>
    public class QuestionService : IQuestionService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="context">数据库上下文</param>
        public QuestionService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// <see cref="IQuestionService.GetByIdAsync(Guid)"/>
        /// </summary>
        public Task<Question> GetByIdAsync(Guid questionId)
        {
            return _context.Questions
                .Include(q => q.Questioner)
                .SingleOrDefaultAsync(q => q.Id == questionId);
        }

        /// <summary>
        /// <see cref="IQuestionService.GetQuery(Expression{Func{Question, bool}}, ISortedParam)"/>
        /// </summary>
        public IQueryable<Question> GetQuery(Expression<Func<Question, bool>> filter = null, ISortedParam sorted = null)
        {
            var query = _context.Questions
                .AsNoTracking()
                .AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (sorted != null)
            {
                query = query.OrderBy(sorted.OrderBy + (sorted.Descending ? " descending" : ""));
            }

            return query;
        }

        /// <summary>
        /// <see cref="IQuestionService.GetAnswersQuery(Guid, ISortedParam)"/>
        /// </summary>
        public IQueryable<Answer> GetAnswersQuery(Guid questionId, ISortedParam sorted = null)
        {
            var query = _context.Answers
                .Include(a => a.Answerer)
                .Include(a => a.Question)
                .Include(a => a.RefAnswer)
                // 限定对应问题
                .Where(a => a.Question.Id == questionId)
                .AsNoTracking();

            // 动态排序
            if (sorted != null)
            {
                query = query.OrderBy(sorted.OrderBy + (sorted.Descending ? " descending" : ""));
            }

            return query;
        }

        /// <summary>
        /// <see cref="IQuestionService.ExistAsync(Guid)"/>
        /// </summary>
        public Task<bool> ExistAsync(Guid questionId)
        {
            // 使用Any比Count > 0效率更高
            return _context.Questions.AnyAsync(q => q.Id == questionId);
        }

        /// <summary>
        /// <see cref="IQuestionService.CreateAsync(User, CreateQuestionParam)"/>
        /// </summary>
        public async Task<Question> CreateAsync(User questioner, CreateQuestionParam dto)
        {
            // 创建新问题
            var question = new Question
            {
                Title = dto.Title,
                // 问题内容
                Content = new Content
                {
                    Text = dto.Text,
                    Images = dto.Images
                },
                Questioner = questioner,
                Category = dto.Category
            };

            // 保存修改
            _context.Add(question);
            await _context.SaveChangesAsync();

            return question;
        }

        /// <summary>
        /// <see cref="IQuestionService.ReplyAsync(Guid, User, ReplyQuestionParam)"/>
        /// </summary>
        public async Task<Answer> ReplyAsync(Guid questionId, User answerer, ReplyQuestionParam dto)
        {
            // 查找问题
            // 因为引用内容不同，故不采用GetById获取question
            Question question = await _context.Questions
                .Include(q => q.Answers)
                .ThenInclude(a => a.Answerer)
                .Include(q => q.Answers)
                .ThenInclude(a => a.RefAnswer)
                .SingleAsync(q => q.Id == questionId);

            // TODO: 暂时对引用问题不存在的问题，作静默处理
            Answer refAnswer = await _context.Answers
                .SingleOrDefaultAsync(a => a.Id == dto.RefAnswerId);

            Answer answer = new Answer
            {
                Answerer = answerer,
                Content = new Content
                {
                    Text = dto.Text,
                    Images = dto.Images
                },
                Question = question,
                RefAnswer = refAnswer
            };
            // 添加回答
            question.Answers.Add(answer);
            // 更新问题的最后回答时间
            question.LastTime = DateTime.Now;
            // 保存问题
            _context.Update(question);
            await _context.SaveChangesAsync();

            return answer;
        }

        /// <summary>
        /// <see cref="IQuestionService.DeleteAsync(Guid)"/>
        /// </summary>
        public async Task DeleteAsync(Guid questionId)
        {
            Question question = await _context.Questions.FindAsync(questionId);
            question.IsDelete = true;

            _context.Update(question);
            await _context.SaveChangesAsync();
        }
    }
}
