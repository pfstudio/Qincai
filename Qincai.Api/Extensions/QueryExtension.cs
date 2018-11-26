using Microsoft.EntityFrameworkCore;
using Qincai.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace Qincai.Api.Extensions
{
    /// <summary>
    /// 查询拓展
    /// </summary>
    public static class QueryExtension
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="query">可查询对象</param>
        /// <param name="pagedParam">分页参数</param>
        /// <returns>分页结果</returns>
        public async static Task<PagedResult<T>> Paged<T>(this IQueryable<T> query, IPagedParam pagedParam)
        {
            // TODO: 此处使用了EF的异步拓展，尚未对非EF查询的对象进行试验。
            var result = await query
                .Skip((pagedParam.Page - 1) * pagedParam.PageSize)
                .Take(pagedParam.PageSize)
                .ToListAsync();
            // 计数
            int totalNum = await query.CountAsync();

            return new PagedResult<T>
            {
                Result = result,
                TotalPage = totalNum == 0 ? 0 : totalNum / pagedParam.PageSize + 1,
                CrtPage = pagedParam.Page,
                PageSize = pagedParam.PageSize
            };
        }
    }
}
