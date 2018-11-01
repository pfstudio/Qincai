using Qincai.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qincai.Api.Dtos
{
    public class AnswerDTO
    {
        public Guid Id { get; set; }
        public UserDTO Answerer { get; set; }
        public Content Content { get; set; }
        public DateTime AnswerTime { get; set; }
    }
}
