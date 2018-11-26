using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Qincai.Api.Dtos
{
    /// <summary>
    /// 创建问题参数
    /// </summary>
    public class CreateQuestionParam
    {
        /// <summary>
        /// 问题标题
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "问题标题不为空")]
        public string Title { get; set; }
        /// <summary>
        /// 问题内容
        /// </summary>
        [Required(ErrorMessage = "问题内容不为空")]
        public string Text { get; set; }
        /// <summary>
        /// 问题包含的图片
        /// </summary>
        [MaxLength(3, ErrorMessage = "问题图片不超过3幅")]
        public List<string> Images { get; set; }
    }
}
