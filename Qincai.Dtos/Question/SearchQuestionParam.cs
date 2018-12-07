using System;
using System.ComponentModel.DataAnnotations;

namespace Qincai.Dtos
{
    /// <summary>
    /// 搜索问题参数
    /// </summary>
    public class SearchQuestionParam : ListQuestionParam
    {
        /// <summary>
        /// 搜索关键词
        /// </summary>
        [MaxLength(20, ErrorMessage = "搜索内容过长")]
        public string Search { get; set; }
        ///// <summary>
        ///// 所需查找问题的标签
        ///// </summary>
        //public string Tag { get; set; }
        /// <summary>
        /// 所需查找问题的分类
        /// </summary>
        [MaxLength(10, ErrorMessage = "类别名过长")]
        public string Category { get; set; }
        /// <summary>
        /// 提问者的Id
        /// </summary>
        public Guid? UserId { get; set; }
    }
}
