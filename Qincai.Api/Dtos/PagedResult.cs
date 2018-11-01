using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qincai.Api.Dtos
{
    /// <summary>
    /// 简化分页操作
    /// </summary>
    /// <typeparam name="T">列表元素类型</typeparam>
    public sealed class PagedResult<T>
    {
        /// <summary>
        /// 分页结果
        /// </summary>
        public List<T> Result { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage { get; set; }
        /// <summary>
        /// 当前页数
        /// </summary>
        public int CrtPage { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }

        public static PagedResult<T> Filter(
            IQueryable<T> query, int page, int pagesize)
        {
            var result = query
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToList();

            int totalNum = query.Count();

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
