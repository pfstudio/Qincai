using System.ComponentModel.DataAnnotations;

namespace Qincai.Api.Dtos
{
    /// <summary>
    /// 用于创建一个新问题
    /// </summary>
    public class CreateQuestion
    {
        /// <summary>
        /// 问题标题
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "问题标题不为空")]
        public string Title { get; set; }
        /// <summary>
        /// 问题内容
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "问题内容不为空")]
        public string Content { get; set; }
    }
}
