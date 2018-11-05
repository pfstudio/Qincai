using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qincai.Api.Dtos
{
    /// <summary>
    /// 微信注册参数
    /// </summary>
    public class WxRegister
    {
        /// <summary>
        /// 3rd-Session Id
        /// </summary>
        public string SessionId { get; set; }
        /// <summary>
        /// 加密后的用户数据
        /// </summary>
        public string EncryptedData { get; set; }
        /// <summary>
        /// 解密用参数
        /// </summary>
        public string Iv { get; set; }
    }
}
