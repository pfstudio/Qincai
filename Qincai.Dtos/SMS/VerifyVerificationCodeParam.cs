using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Qincai.Dtos
{
    /// <summary>
    /// 检验验证码参数
    /// </summary>
    public class VerifyVerificationCodeParam
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "手机号不为空")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "验证码不为空")]
        public string VerificationCode { get; set; }
    }
}
