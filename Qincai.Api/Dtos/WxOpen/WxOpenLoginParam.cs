using System.ComponentModel.DataAnnotations;

namespace Qincai.Dtos
{
    /// <summary>
    /// 微信登录参数
    /// </summary>
    public class WxOpenLoginParam
    {
        /// <summary>
        /// wx.login 返回的code
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "code 不为空")]
        public string Code { get; set; }
    }
}
