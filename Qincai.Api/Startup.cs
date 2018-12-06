﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Qincai.Dtos;
using Qincai.Api.Extensions;
using Qincai.Models;
using Qincai.Services;
using Qincai.Api.Utils;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.RegisterServices;
using System;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Qincai.Api
{
    /// <summary>
    /// 配置类
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="configuration">配置对象</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 配置对象
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services">服务集合</param>
        public void ConfigureServices(IServiceCollection services)
        {
            #region 配置数据库连接
            services.AddDbContext<ApplicationDbContext>(options
            => options.UseSqlServer(Configuration.GetConnectionString("Local")));
            //=> options.UseMySql(Configuration.GetConnectionString("MySQL")));
            //=> options.UseInMemoryDatabase("Qincai"));
            #endregion

            #region 配置外部服务参数
            // 配置小程序参数
            services.Configure<WxOpenConfig>(Configuration.GetSection("WxOpen"));
            // 配置七牛云参数
            services.Configure<QiniuConfig>(Configuration.GetSection("QiniuConfig"));
            #endregion

            #region 注入依赖的服务
            // ImageService作为上游服务
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();
            #endregion

            #region 配置AutoMapper映射关系
            services.AddAutoMapper(options =>
            {
                options.CreateMap<Content, ContentDto>();
                options.CreateMap<Question, QuestionDto>()
                    .ForMember(dto => dto.AnswerNum, opt => opt.MapFrom(src => src.Answers.Count()));
                options.CreateMap<Answer, AnswerDto>()
                    // 配置问题引用的映射
                    .ForMember(dto => dto.RefAnswer, opt => opt.MapFrom(src => src.RefAnswer));
                options.CreateMap<Answer, AnswerWithQuestionDto>()
                    .ForMember(dto => dto.RefAnswer, opt => opt.MapFrom(src => src.RefAnswer))
                    .ForMember(dto => dto.Question, opt => opt.MapFrom(src => src.Question));
                options.CreateMap<User, UserDto>();
            });
            #endregion

            #region 配置JWT认证
            // 配置JWT参数
            var jwtConfigSection = Configuration.GetSection("JwtConfig:WxOpen");
            services.Configure<JwtConfig>(jwtConfigSection);
            JwtConfig jwtConfig = jwtConfigSection.Get<JwtConfig>();
            // 添加JWT认证
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // JWT参数配置
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // 客户端
                    ValidateAudience = true,
                    ValidAudience = jwtConfig.Audience,
                    // 服务器
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,
                    // 签名
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = jwtConfig.Key,
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
            });
            #endregion

            // 配置跨域访问
            services.AddCors();

            // 添加MVC服务
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    // 不循环序列化
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // 配置授权策略
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicies.Ownered, policy =>
                    policy.AddRequirements(new OwneredRequirement()));
            });
            services.AddSingleton<IAuthorizationHandler, OwneredAuthorizationHandler>();

            // 添加Swagger
            services.ConfigureSwagger();

            // 配置缓存
            // TODO：改用Redis
            services.AddMemoryCache();

            // 配置Senparc服务
            services.AddSenparcGlobalServices(Configuration)
                .AddSenparcWeixinServices(Configuration);
        }

        /// <summary>
        /// 配置App Pipelines
        /// </summary>
        /// <param name="app">app builder</param>
        /// <param name="env">环境参数</param>
        /// <param name="senparcSetting">senparc 设置</param>
        /// <param name="senparcWeixinSetting">senparc 微信设置</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            // 初始化数据库
            SeedData.InitDatabase(app.ApplicationServices.CreateScope().ServiceProvider);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseHsts();
            //}

            //app.UseHttpsRedirection();

            // 使用Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Qincai API v1");
            });

            // 启用认证
            app.UseAuthentication();

            app.UseMvc();

            // 启动 CO2NET 全局注册，必须！
            IRegisterService register = RegisterService.Start(env, senparcSetting.Value)
                .UseSenparcGlobal();
            //微信全局注册，必须！
            register.UseSenparcWeixin(senparcWeixinSetting.Value, senparcSetting.Value);
        }
    }
}
