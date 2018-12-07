namespace Qincai.Dtos
{
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
