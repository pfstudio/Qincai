﻿namespace Qincai.Dtos
{
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
