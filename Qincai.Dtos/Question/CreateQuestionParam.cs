using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Qincai.Dtos
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
        ///// <summary>
        ///// 问题的标签
        ///// </summary>
        //[MaxLength(5, ErrorMessage = "问题标签不超过5个")]
        //public List<string> Tags { get; set; }
        /// <summary>
        /// 问题的分类
        /// </summary>
        public string Category { get; set; }
    }
}
