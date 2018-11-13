using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Qincai.Api.Services;
using System;
using System.Security.Claims;
using System.Text;

namespace Qincai.Api.Utils
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public SymmetricSecurityKey Key
        {
            get
            {
                return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            }
        }

        public void Configure(JwtBearerOptions options)
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // 客户端
                ValidateAudience = true,
                ValidAudience = Audience,
                // 服务器
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                // 签名
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = Key,
                // 有效期
                ValidateLifetime = true
            };
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    if (!Guid.TryParse(context.Principal.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
                    {
                        context.Fail("异常的token");
                    }
                    IUserService userService = context.Request.HttpContext.RequestServices.GetRequiredService<IUserService>();
                    if (!await userService.ExistByIdAsync(userId))
                    {
                        context.Fail("授权用户不存在");
                    }
                }
            };
        }
    }
}
