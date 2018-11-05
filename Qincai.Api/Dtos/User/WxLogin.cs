using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qincai.Api.Dtos
{
    /// <summary>
    /// 微信登录参数
    /// </summary>
    public class WxLogin
    {
        /// <summary>
        /// wx.login 返回的code
        /// </summary>
        public string Code { get; set; }
    }
}
