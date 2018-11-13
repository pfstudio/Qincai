using System.ComponentModel.DataAnnotations;

namespace Qincai.Api.Dtos
{
    /// <summary>
    /// 小程序认证
    /// </summary>
    public class WxOpenAuthorize
    {
        /// <summary>
        /// 3rd-session
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "SessionId 不为空")]
        public string SessionId { get; set; }
    }
}
