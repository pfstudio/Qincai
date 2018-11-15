using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Qincai.Api.Dtos;
using Qincai.Api.Extensions;
using Qincai.Api.Models;
using Qincai.Api.Services;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.RegisterServices;

namespace Qincai.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options
            //=> options.UseSqlServer(Configuration.GetConnectionString("Local")));
            //=> options.UseMySql(Configuration.GetConnectionString("MySQL")));
            => options.UseInMemoryDatabase("Qincai"));

            // 注入依赖的服务
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IQuestionService, QuestionService>();

            // 添加类型映射配置
            services.AddAutoMapper(options =>
            {
                options.CreateMap<Question, QuestionDto>();
                options.CreateMap<Answer, AnswerDto>()
                    // 配置问题引用的映射
                    .ForMember(a => a.RefAnswer, opt => opt.MapFrom(a => a.RefAnswer));
                options.CreateMap<User, UserDto>();
            });

            // 配置小程序参数
            services.ConfigureWxOpen(Configuration);
            // 配置JWT参数
            var jwtConfig = services.ConfigureJwt(Configuration);
            // 添加JWT认证
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => jwtConfig.Configure(options));

            // Cors跨域
            services.AddCors();

            // 添加MVC服务
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    // 不循环序列化
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // 添加Swagger
            services.ConfigureSwagger();

            // 配置缓存
            // TODO：改用Redis
            services.AddMemoryCache();

            // 配置Senparc服务
            services.AddSenparcGlobalServices(Configuration)
                .AddSenparcWeixinServices(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            //SeedData.InitDatabase(app.ApplicationServices.CreateScope().ServiceProvider);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseHsts();
            //}

            //app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Qincai API v1");
            });

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
