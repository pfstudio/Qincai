using System;

namespace Qincai.Api.Models
{
    /// <summary>
    /// 回答实体
    /// </summary>
    public class Answer
    {
        /// <summary>
        /// 回答Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 回答者
        /// </summary>
        public User Answerer { get; set; }
        /// <summary>
        /// 所属问题
        /// </summary>
        public Question Question { get; set; }
        /// <summary>
        /// 回答内容
        /// </summary>
        public Content Content { get; set; }
        /// <summary>
        /// 回答时间
        /// </summary>
        public DateTime AnswerTime { get; set; }
        /// <summary>
        /// 引用的回答
        /// </summary>
        public Answer RefAnswer { get; set; }
    }
}
