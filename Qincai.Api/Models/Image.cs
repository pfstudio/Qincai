using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qincai.Api.Models
{
    public class Image
    {
        /// <summary>
        /// 图片ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 上传者
        /// </summary>
        public User Uploader { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string SoureUrl { get; set; }

        /// <summary>
        /// 创建图片对象
        /// </summary>
        /// <param name="name">图片别名</param>
        /// <param name="uploader">上传者</param>
        /// <param name="sourceUrl">图片地址</param>
        /// <returns></returns>
        public static Image Create(string name,User uploader,string sourceUrl)
        {
            return new Image
            {
                Id = Guid.NewGuid(),
                Uploader = uploader,
                CreateTime = DateTime.Now,
                SoureUrl = sourceUrl
            };
        }
    }
}
