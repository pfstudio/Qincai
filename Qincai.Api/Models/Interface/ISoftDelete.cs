namespace Qincai.Models
{
    /// <summary>
    /// 软删除约束
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// 软删除标志
        /// </summary>
        bool IsDelete { get; set; }
    }
}
