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
    /// 回答相关服务接口
    /// </summary>
    public interface IAnswerService
    {
        /// <summary>
        /// 根据Id获取回答
        /// </summary>
        /// <param name="answerId">回答Id</param>
        /// <returns>回答实体</returns>
        Task<Answer> GetByIdAsync(Guid answerId);
        /// <summary>
        /// 查找回答
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="sorted">排序参数</param>
        /// <returns>回答的可查询对象</returns>
        IQueryable<Answer> GetQuery(Expression<Func<Answer, bool>> filter = null, ISortedParam sorted = null);
        /// <summary>
        /// 软删除回答
        /// </summary>
        /// <param name="answerId">回答Id</param>
        Task DeleteAsync(Guid answerId);
    }

    /// <summary>
    /// 回答服务
    /// </summary>
    public class AnswerService : IAnswerService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="context">数据库上下文</param>
        public AnswerService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// <see cref="IAnswerService.GetByIdAsync(Guid)"/>
        /// </summary>
        public Task<Answer> GetByIdAsync(Guid answerId)
        {
            return _context.Answers
                .Include(a => a.Answerer)
                .Include(a => a.RefAnswer)
                .Include(a => a.Question)
                .AsNoTracking()
                .SingleOrDefaultAsync(a => a.Id == answerId);
        }

        /// <summary>
        /// <see cref="IAnswerService.GetQuery(Expression{Func{Answer, bool}}, ISortedParam)"/>
        /// </summary>
        public IQueryable<Answer> GetQuery(Expression<Func<Answer, bool>> filter = null, ISortedParam sorted = null)
        {
            var query = _context.Answers
                .Include(a => a.Answerer)
                .Include(a => a.Question)
                .Include(a => a.RefAnswer)
                .AsNoTracking();

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
        /// <see cref="IAnswerService.DeleteAsync(Guid)"/>
        /// </summary>
        public async Task DeleteAsync(Guid answerId)
        {
            Answer answer = await _context.Answers.FindAsync(answerId);
            answer.IsDelete = true;

            _context.Update(answer);
            await _context.SaveChangesAsync();
        }
    }
}
