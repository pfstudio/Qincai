using System;

namespace Qincai.Models
{
    /// <summary>
    /// 回答实体
    /// </summary>
    public class Answer : SoftDeleteEntity, IHasOwner<User>
    {
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
        public DateTime AnswerTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 引用的回答
        /// </summary>
        public Answer RefAnswer { get; set; }

        /// <summary>
        /// 资源拥有者
        /// </summary>
        public User Owner => Answerer;
    }
}
