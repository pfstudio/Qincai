using System;
using System.Collections.Generic;

namespace Qincai.Models
{
    /// <summary>
    /// 问题实体
    /// </summary>
    public class Question : IHasOwner<User>, ISoftDelete
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
        public Content Content { get; set; }
        /// <summary>
        /// 问题类别
        /// </summary>
        public string Category { get; set; }
        ///// <summary>
        ///// 问题标签
        ///// </summary>
        //public List<string> Tags { get; set; }
        /// <summary>
        /// 问题的回答
        /// </summary>
        public List<Answer> Answers { get; set; } = new List<Answer>();
        /// <summary>
        /// 提问者
        /// </summary>
        public User Questioner { get; set; }
        /// <summary>
        /// 提问时间
        /// </summary>
        public DateTime QuestionTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 最后回答时间
        /// </summary>
        public DateTime LastTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 软删除标记
        /// </summary>
        public bool IsDelete { get; set; } = false;

        /// <summary>
        /// 资源所有者
        /// </summary>
        public User Owner => Questioner;

    }
}
