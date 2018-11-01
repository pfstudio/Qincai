using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qincai.Api.Models
{
    public class Answer
    {
        public Guid Id { get; set; }
        public User Answerer { get; set; }
        public Question Question { get; set; }
        public Content Content { get; set; }
        public DateTime AnswerTime { get; set; }
        //public Answer RefAnswer { get; set; }
    }
}
