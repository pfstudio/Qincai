namespace Qincai.Models
{
    /// <summary>
    /// 用户实体
    /// </summary>
    public class User : SoftDeleteEntity
    {
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; } = UserRole.User;
        /// <summary>
        /// 头像Url
        /// </summary>
        public string AvatarUrl { get; set; }
        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string WxOpenId { get; set; }
    }

    /// <summary>
    /// 用户角色
    /// </summary>
    public static class UserRole
    {
        /// <summary>
        /// 用户
        /// </summary>
        public const string User = "用户";
        /// <summary>
        /// 管理员
        /// </summary>
        public const string Admin = "管理员";
    }
}
