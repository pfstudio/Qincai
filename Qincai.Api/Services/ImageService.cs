using Microsoft.Extensions.Options;
using Qincai.Dtos;
using Qincai.Models;
using Qincai.Api.Utils;
using Qiniu.IO.Model;
using Qiniu.Util;
using System;

namespace Qincai.Services
{
    /// <summary>
    /// 图片相关服务接口
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// 创建上传Token
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns>上传相关参数</returns>
        ImageToken CreateToken(User user);
        /// <summary>
        /// 转换为绝对路径
        /// </summary>
        /// <param name="url">相对路径</param>
        /// <returns>图片的绝对路径</returns>
        string ConvertToAbsolute(string url);
    }

    /// <summary>
    /// 图片服务
    /// </summary>
    public class ImageService: IImageService
    {
        private readonly QiniuConfig _qiniuConfig;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="qiniuConfig">七牛云配置文件</param>
        public ImageService(IOptions<QiniuConfig> qiniuConfig)
        {
            _qiniuConfig = qiniuConfig.Value;
        }

        /// <summary>
        /// <see cref="IImageService.CreateToken(User)"/>
        /// </summary>
        public ImageToken CreateToken(User user)
        {
            Mac mac = new Mac(_qiniuConfig.ACCESS_KEY, _qiniuConfig.SECRET_KEY);
            Auth auth = new Auth(mac);
            // 图片的标识
            // userId/datetime-random
            string key = user.Id.ToString() + "/" + DateTime.Now.ToString("yyyyMMddhhmmss")
                + "-" + new Random().Next(100, 999).ToString();
            // 创建上传策略
            PutPolicy putPolicy = new PutPolicy()
            {
                // 使用后缀
                Scope = _qiniuConfig.ImageBucket.Name + ":" + key
            };
            // 设置有效时限
            putPolicy.SetExpires(120);
            // 限制图片大小
            putPolicy.FsizeLimit = 10 * 1024 * 1024;
            // 七牛云上传成功后对服务器的回调
            // putPolicy.CallbackUrl ="";

            //返回生成的token模型
            return new ImageToken()
            {
                // 上传Token
                Token = auth.CreateUploadToken(putPolicy.ToJsonString()),
                // 图片标识
                Key = key,
                // 上传域名
                UploadDomain = _qiniuConfig.ImageBucket.UploadDomain
            };
        }

        /// <summary>
        /// <see cref="IImageService.ConvertToAbsolute(string)"/>
        /// </summary>
        public string ConvertToAbsolute(string url)
        {
            return $"https://{_qiniuConfig.ImageBucket.DownloadDomain}/{url}";
        }
    }
}
