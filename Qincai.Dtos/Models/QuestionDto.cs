using System;

namespace Qincai.Dtos
{
    /// <summary>
    /// 问题Dto
    /// </summary>
    public class QuestionDto
    {
        /// <summary>
        /// 问题Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 问题标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 问题内容
        /// </summary>
        public ContentDto Content { get; set; }
        /// <summary>
        /// 提问者
        /// </summary>
        public UserDto Questioner { get; set; }
        /// <summary>
        /// 提问时间
        /// </summary>
        public DateTime QuestionTime { get; set; }
        /// <summary>
        /// 最后一次回答时间
        /// </summary>
        public DateTime LastTime { get; set; }
        ///// <summary>
        ///// 问题的标签
        ///// </summary>
        //public List<string> Tags { get; set; }
        /// <summary>
        /// 问题类别
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 回答数
        /// </summary>
        public int AnswerNum { get; set; }
    }
}
