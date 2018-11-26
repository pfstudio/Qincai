using System;
using System.Collections.Generic;

namespace Qincai.Api.Models
{
    /// <summary>
    /// 问题实体
    /// </summary>
    public class Question
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
        //public List<string> Tags { get; set; }
        /// <summary>
        /// 问题的回答
        /// </summary>
        public List<Answer> Answers { get; set; }
        /// <summary>
        /// 提问者
        /// </summary>
        public User Questioner { get; set; }
        /// <summary>
        /// 提问时间
        /// </summary>
        public DateTime QuestionTime { get; set; }
        /// <summary>
        /// 最后回答时间
        /// </summary>
        public DateTime LastTime { get; set; }
        //public QuestionType Type { get; set; }
        //public QuestionStatus Status { get; set; }
    }

    //public enum QuestionType
    //{
    //    OnlyTeacher,
    //    Public
    //}

    //public enum QuestionStatus
    //{
    //    Open,
    //    Closed
    //}
}
