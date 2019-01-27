using System.ComponentModel.DataAnnotations;

namespace Qincai.Dtos
{
    /// <summary>
    /// 发送验证码参数
    /// </summary>
    public class SendVerificationCodeParam
    {
        /// <summary>
        /// 欲发送的电话号码
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "电话号码不为空")]
        public string PhoneNumber { get; set; }
    }
}
