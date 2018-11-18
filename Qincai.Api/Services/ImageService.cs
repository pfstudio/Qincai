using Microsoft.Extensions.Options;
using Qincai.Api.Dtos;
using Qincai.Api.Models;
using Qincai.Api.Utils;
using Qiniu.Storage;
using Qiniu.Util;
using System;

namespace Qincai.Api.Services
{
    public interface IImageService
    {
        ImageToken CreateToken(User user);
        string ConvertToAbsolute(string url);
    }

    public class ImageService: IImageService
    {
        private readonly QiniuConfig _qiniuConfig;

        public ImageService(IOptions<QiniuConfig> qiniuConfig)
        {
            _qiniuConfig = qiniuConfig.Value;
        }

        public string ConvertToAbsolute(string url)
        {
            return $"https://{_qiniuConfig.ImageBucket.DownloadDomain}/{url}";
        }

        public ImageToken CreateToken(User user)
        {
            Mac mac = new Mac(_qiniuConfig.ACCESS_KEY, _qiniuConfig.SECRET_KEY);
            Auth auth = new Auth(mac);
            string key = user.Id.ToString() + "/" + DateTime.Now.ToString("yyyyMMddhhmmss")
                + "-" + new Random().Next(100, 999).ToString();
            PutPolicy putPolicy = new PutPolicy()
            {
                //使用后缀
                Scope = _qiniuConfig.ImageBucket.Name + ":" + key
            };
            putPolicy.SetExpires(120);
            //限制图片大小
            putPolicy.FsizeLimit = 10 * 1024 * 1024;
            //七牛云上传成功后对服务器的回调
            // putPolicy.CallbackUrl ="";
            string jstr = putPolicy.ToJsonString();

            //返回生成的token模型
            return new ImageToken()
            {
                Token = auth.CreateUploadToken(jstr),
                Key = key,
                UploadDomain = _qiniuConfig.ImageBucket.UploadDomain
            };
        }
    }
}
