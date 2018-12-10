using System;

namespace Qincai.Models
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// 实体主键Id
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
    }

    /// <summary>
    /// 带有软删除的实体
    /// </summary>
    public class SoftDeleteEntity : BaseEntity, ISoftDelete
    {
        /// <summary>
        /// 软删除的标识
        /// </summary>
        public bool IsDelete { get; set; } = false;
    }
}
