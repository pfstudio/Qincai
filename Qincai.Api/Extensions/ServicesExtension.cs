using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Qincai.Api.Utils;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;

namespace Qincai.Api.Extensions
{
    public static class ServicesExtension
    {
        public static WxOpenConfig ConfigureWxOpen(this IServiceCollection services, IConfiguration configuration)
        {
            var wxOpenConfigSection = configuration.GetSection("WxOpen");
            services.Configure<WxOpenConfig>(wxOpenConfigSection);

            return wxOpenConfigSection.Get<WxOpenConfig>();
        }

        public static JwtConfig ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfigSection = configuration.GetSection("JwtConfig:WxOpen");
            services.Configure<JwtConfig>(jwtConfigSection);

            return jwtConfigSection.Get<JwtConfig>();
        }

        public static QiniuConfig ConfigureQiniu(this IServiceCollection services, IConfiguration configuration)
        {
            var qiniuConfigSection = configuration.GetSection("QiniuConfig");
            services.Configure<Utils.QiniuConfig>(qiniuConfigSection);

            return qiniuConfigSection.Get<QiniuConfig>();
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Qincai API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "请键入JWT Token，格式为'Bearer '+你的Token。",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
                // TODO: 对于文件不存在情况下的异常处理
                var filePath = Path.Combine(AppContext.BaseDirectory, "Qincai.Api.xml");
                c.IncludeXmlComments(filePath);
            });
            return services;
        }
    }
}
