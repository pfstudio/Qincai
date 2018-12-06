namespace Qincai.Dtos
{
    /// <summary>
    /// JWT配置参数
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 颁发者
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 接受者
        /// </summary>
        public string Audience { get; set; }
    }

    /// <summary>
    /// 微信小程序配置参数
    /// </summary>
    public class WxOpenConfig
    {
        /// <summary>
        /// 微信小程序 AppId
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 微信小程序 Secert
        /// </summary>
        public string Secret { get; set; }
    }

    /// <summary>
    /// 七牛云配置参数
    /// </summary>
    public class QiniuConfig
    {
        /// <summary>
        /// AK
        /// </summary>
        public string ACCESS_KEY { get; set; }
        /// <summary>
        /// SK
        /// </summary>
        public string SECRET_KEY { get; set; }
        /// <summary>
        /// 存储桶
        /// </summary>
        public Bucket ImageBucket { get; set; }

        /// <summary>
        /// Bucket配置参数
        /// </summary>
        public class Bucket
        {
            /// <summary>
            /// Bucket名
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 上传域名
            /// </summary>
            public string UploadDomain { get; set; }
            /// <summary>
            /// 下载域名
            /// </summary>
            public string DownloadDomain { get; set; }
        }
    }
}
