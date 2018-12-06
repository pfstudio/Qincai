using Microsoft.AspNetCore.Authorization;
using Qincai.Api.Extensions;
using Qincai.Models;
using System.Threading.Tasks;

namespace Qincai.Api.Utils
{
    /// <summary>
    /// 资源拥有者授权Handler
    /// </summary>
    public class OwneredAuthorizationHandler :
        AuthorizationHandler<OwneredRequirement, IHasOwner<User>>
    {
        /// <summary>
        /// 重载授权过程
        /// </summary>
        /// <param name="context">认证上下文</param>
        /// <param name="requirement">要求</param>
        /// <param name="resource">资源</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwneredRequirement requirement, IHasOwner<User> resource)
        {
            User owner = resource.Owner;
            if (owner.Role == "admin" || owner.Id == context.User.GetUserId())
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 资源拥有者 要求
    /// </summary>
    public class OwneredRequirement : IAuthorizationRequirement { }
}
