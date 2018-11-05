using Qincai.Api.Models;
using System;

namespace Qincai.Api.Dtos
{
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
        public Content Content { get; set; }
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
    }
}
