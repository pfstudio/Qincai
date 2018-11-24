using Xunit;
using Qincai.Api.Services;
using Qincai.Api.Utils;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Qincai.Api;
using Qincai.Api.Controllers;
using Qincai.Api.Models;
using System.Net.Http;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace Qincai.Api.Tests
{
    public class ImageTest
    {
        private readonly IOptions<QiniuConfig> _qiniuConfig;
        private readonly IConfigurationRoot _configuration;
        private readonly IServiceProvider _serviceProvider;

        private readonly ITestOutputHelper _output;

        public ImageTest(ITestOutputHelper output)
        {
            _output = output;

            // 读取Config
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            _configuration = configBuilder.Build();

            //配置DI
            var services = new ServiceCollection();
            services.AddOptions();

            services.Configure<QiniuConfig>(_configuration.GetSection("QiniuConfig"));
            _serviceProvider = services.BuildServiceProvider();

            _qiniuConfig = _serviceProvider.GetService<IOptions<QiniuConfig>>();
        }

        [Fact]
        public void ConvertToAbsoluteTest()
        {
            string source = Guid.NewGuid().ToString() + "/20181122164941-458";
            var Exp = $"https://" + _qiniuConfig.Value.ImageBucket.DownloadDomain + "/" + source;

            var result = new ImageService(_qiniuConfig).ConvertToAbsolute(source);
            
            Assert.Equal(result, Exp);
        }

        [Fact]
        public async void CreateToken()
        {
            //生成Token
            User user = new User()
            {
                AvatarUrl = null,
                Id = Guid.NewGuid(),
                Name = "咕咕咕",
                WxOpenId = null
            };
            var token = new ImageService(_qiniuConfig).CreateToken(user);


            //构造Http请求
            HttpClient client = new HttpClient();

            MultipartFormDataContent content = new MultipartFormDataContent
            {
                { new StringContent(token.Token), "token" },
                { new StringContent(token.Key), "key" },
                { new ByteArrayContent(new byte[100]), "file" ,token.Key}
            };

            //等待响应
            var response = await client.PostAsync(_qiniuConfig.Value.ImageBucket.UploadDomain, content);

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
