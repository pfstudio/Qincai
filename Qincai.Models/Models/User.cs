using System;

namespace Qincai.Models
{
    /// <summary>
    /// 用户实体
    /// </summary>
    public class User : ISoftDelete
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// 头像Url
        /// </summary>
        public string AvatarUrl { get; set; }
        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string WxOpenId { get; set; }
        /// <summary>
        /// 软删除标志
        /// </summary>
        public bool IsDelete { get; set; } = false;
    }
}
