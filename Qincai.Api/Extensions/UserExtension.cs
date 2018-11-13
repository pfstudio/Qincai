using Qincai.Api.Models;
using System;
using System.Security.Claims;

namespace Qincai.Api.Extensions
{
    public static class UserExtension
    {
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            Guid userId = Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
            return userId;
        }
    }
}
