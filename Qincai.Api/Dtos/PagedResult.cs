using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qincai.Api.Dtos
{
    /// <summary>
    /// 分页结果
    /// </summary>
    /// <typeparam name="T">列表元素类型</typeparam>
    public sealed class PagedResult<T>
    {
        /// <summary>
        /// 分页结果
        /// </summary>
        public IEnumerable<T> Result { get; private set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage { get; private set; }
        /// <summary>
        /// 当前页数
        /// </summary>
        public int CrtPage { get; private set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// 封闭构造
        /// </summary>
        private PagedResult() {}

        /// <summary>
        /// 创建分页结果
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <param name="page">页数</param>
        /// <param name="pagesize">每页数量</param>
        /// <returns><see cref="PagedResult{T}"/></returns>
        public static async Task<PagedResult<T>> CreateAsync(
            IQueryable<T> query, int page, int pagesize)
        {
            // TODO: 此处使用了EF的异步拓展，尚未对非EF查询的对象进行试验。
            var result = await query
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

            int totalNum = await query.CountAsync();

            return new PagedResult<T>
            {
                Result = result,
                TotalPage = totalNum == 0 ? 0 : totalNum / pagesize + 1,
                CrtPage = page,
                PageSize = pagesize
            };
        }
    }
}
