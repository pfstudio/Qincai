using System;

namespace Qincai.Api.Dtos
{
    /// <summary>
    /// 一般回答
    /// </summary>
    public class AnswerDto
    {
        /// <summary>
        /// 问题Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 回答者
        /// </summary>
        public UserDto Answerer { get; set; }
        /// <summary>
        /// 回答内容
        /// </summary>
        public ContentDto Content { get; set; }
        /// <summary>
        /// 回答时间
        /// </summary>
        public DateTime AnswerTime { get; set; }
        /// <summary>
        /// 引用的回答
        /// </summary>
        public RefAnswerDto RefAnswer { get; set; }
    }

    /// <summary>
    /// 被引用的回答
    /// </summary>
    public class RefAnswerDto
    {
        /// <summary>
        /// 问题Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 回答者
        /// </summary>
        public UserDto Answerer { get; set; }
        /// <summary>
        /// 回答内容
        /// </summary>
        public ContentDto Content { get; set; }
    }

    /// <summary>
    /// 包含所属问题的回答
    /// </summary>
    public class AnswerWithQuestionDto: AnswerDto
    {
        /// <summary>
        /// 所回答的问题
        /// </summary>
        public QuestionDto Question { get; set; }
    }
}
