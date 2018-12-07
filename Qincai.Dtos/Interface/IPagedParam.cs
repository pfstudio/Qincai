namespace Qincai.Dtos
{
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
}
