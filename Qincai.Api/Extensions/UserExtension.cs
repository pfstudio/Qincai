using System;
using System.Security.Claims;

namespace Qincai.Api.Extensions
{
    /// <summary>
    /// 用户信息相关的拓展方法
    /// </summary>
    public static class UserExtension
    {
        /// <summary>
        /// 解析User Id，已由中间件确保用户存在
        /// </summary>
        /// <param name="claimsPrincipal">用户令牌</param>
        /// <returns>(Guid)User Id</returns>
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            Guid userId = Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
            return userId;
        }
    }
}
