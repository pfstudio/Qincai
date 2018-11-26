using Microsoft.EntityFrameworkCore;
using Qincai.Api;
using Qincai.Dtos;
using Qincai.Models;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Qincai.Services
{
    /// <summary>
    /// 回答相关服务接口
    /// </summary>
    public interface IAnswerService
    {
        /// <summary>
        /// 查找回答
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="sorted">排序参数</param>
        /// <returns>回答的可查询对象</returns>
        IQueryable<Answer> GetQuery(Expression<Func<Answer, bool>> filter = null, ISortedParam sorted = null);
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
    }
}
