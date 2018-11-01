using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qincai.Api.Dtos
{
    public sealed class PagedResult<T>
    {
        public List<T> Result { get; set; }
        public int TotalPage { get; set; }
        public int CrtPage { get; set; }
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
