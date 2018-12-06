namespace Qincai.Models
{
    /// <summary>
    /// 资源拥有者约束
    /// </summary>
    /// <typeparam name="T">拥有者类型</typeparam>
    public interface IHasOwner<T>
    {
        /// <summary>
        /// 拥有者
        /// </summary>
        T Owner { get; }
    }
}
