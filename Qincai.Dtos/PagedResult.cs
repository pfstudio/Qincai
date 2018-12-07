using System.Collections.Generic;

namespace Qincai.Dtos
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
        public IEnumerable<T> Result { get; set; }
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
    }
}
