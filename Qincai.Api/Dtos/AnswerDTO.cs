using Qincai.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qincai.Api.Dtos
{
    public class AnswerDTO
    {
        /// <summary>
        /// 问题Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 回答者
        /// </summary>
        public UserDTO Answerer { get; set; }
        /// <summary>
        /// 回答内容
        /// </summary>
        public Content Content { get; set; }
        /// <summary>
        /// 回答时间
        /// </summary>
        public DateTime AnswerTime { get; set; }
    }
}
