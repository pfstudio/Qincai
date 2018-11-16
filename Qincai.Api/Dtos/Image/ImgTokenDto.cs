using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qincai.Api.Dtos
{
    public class ImgTokenDto
    {
        /// <summary>
        /// 上传Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 允许的文件名
        /// </summary>
        public string Scope { get; set; }
        /// <summary>
        /// 上传图片的域名
        /// </summary>
        public string UploadDomain { get; set; }
    }
}
