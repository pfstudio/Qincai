namespace Qincai.Api.Utils
{
    /// <summary>
    /// 定义的授权策略类型
    /// </summary>
    public static class AuthorizationPolicies
    {
        /// <summary>
        /// 资源拥有者策略
        /// </summary>
        public const string Ownered = nameof(Ownered);
        /// <summary>
        /// 管理员权限策略
        /// </summary>
        public const string Admin = nameof(Admin);
    }
}
