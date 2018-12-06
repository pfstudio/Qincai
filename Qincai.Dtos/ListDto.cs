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

    /// <summary>
    /// 分页参数
    /// </summary>
    public interface IPagedParam
    {
        /// <summary>
        /// 当前页数
        /// </summary>
        int Page { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        int PageSize { get; set; }
    }

    /// <summary>
    /// 排序参数
    /// </summary>
    public interface ISortedParam
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        string OrderBy { get; set; }
        /// <summary>
        /// 是否降序
        /// </summary>
        bool Descending { get; set; }
    }
}
