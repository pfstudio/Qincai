﻿using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;

namespace Qincai.Api.Extensions
{
    /// <summary>
    /// StartUp中注入服务的拓展方法
    /// </summary>
    public static class ServicesExtension
    {
        /// <summary>
        /// 配置Swagger
        /// </summary>
        /// <param name="services">服务集合</param>
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
