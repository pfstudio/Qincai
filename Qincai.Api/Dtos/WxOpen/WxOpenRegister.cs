using System.ComponentModel.DataAnnotations;

namespace Qincai.Api.Dtos
{
    /// <summary>
    /// 微信注册参数
    /// </summary>
    public class WxOpenRegister
    {
        /// <summary>
        /// 3rd-Session Id
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "sessionId 不为空")]
        public string SessionId { get; set; }
        /// <summary>
        /// 加密后的用户数据
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "用户数据不为空")]
        public string EncryptedData { get; set; }
        /// <summary>
        /// 解密用参数
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "解密向量不为空")]
        public string Iv { get; set; }
    }
}
