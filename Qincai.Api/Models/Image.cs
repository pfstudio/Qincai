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
        public Guid UploaderId { get; set; }
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
        /// <param name="uploaderId">上传者</param>
        /// <param name="sourceUrl">图片地址</param>
        /// <returns></returns>
        public static Image Create(Guid uploaderId,string sourceUrl)
        {
            return new Image
            {
                Id = Guid.NewGuid(),
                UploaderId = uploaderId,
                CreateTime = DateTime.Now,
                SoureUrl = sourceUrl
            };
        }
    }
}
