using System.ComponentModel.DataAnnotations;

namespace Qincai.Api.Dtos
{
    /// <summary>
    /// 用于回答问题
    /// </summary>
    public class ReplyQuestion
    {
        /// <summary>
        /// 回答内容
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "回答内容不为空")]
        public string Content { get; set; }
    }
}
