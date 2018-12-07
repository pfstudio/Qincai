using System.ComponentModel.DataAnnotations;

namespace Qincai.Dtos
{
    /// <summary>
    /// 微信注册参数
    /// </summary>
    public class WxOpenRegisterParam
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
        /// <summary>
        /// 用户自定义昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户自定义头像
        /// </summary>
        public string AvatarUrl { get; set; }
    }
}
