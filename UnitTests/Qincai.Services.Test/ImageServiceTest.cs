using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Qincai.Dtos;
using Qincai.Models;
using Qiniu.IO;
using Qiniu.RS;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Qincai.Services.Test
{
    public class ImageServiceTest
    {
        private readonly QiniuConfig qiniuConfig;
        private readonly IImageService _imageService;

        public ImageServiceTest()
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets("56E7CA17-9DC4-419A-9E8B-C23C584BDD06")
                .Build();
            qiniuConfig = configuration.GetSection("QiniuConfig").Get<QiniuConfig>();
            _imageService = new ImageService(Options.Create(qiniuConfig));
        }

        [Fact]
        public async Task CreateTokenTest()
        {
            User user = new User();
            var imageUploadToken = _imageService.CreateToken(user);
            UploadManager um = new UploadManager();
            var result = await um.UploadFileAsync("test.png", imageUploadToken.Key, imageUploadToken.Token);
            result.Code.Should().Be(200, "上传图片成功");
            // 清理上传的图片
            BucketManager bm = new BucketManager(new Mac(qiniuConfig.ACCESS_KEY, qiniuConfig.SECRET_KEY));
            bm.Delete(qiniuConfig.ImageBucket.Name, imageUploadToken.Key);
        }

        [Fact]
        public void ConvertToAbsoluteTest()
        {
            string key = "test";
            string result = _imageService.ConvertToAbsolute(key);
            result.Should().Be($"https://{qiniuConfig.ImageBucket.DownloadDomain}/{key}");
        }
    }
}
