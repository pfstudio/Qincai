using Qincai.Api.Models;
using System;

namespace Qincai.Api.Dtos
{
    public class QuestionDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Content Content { get; set; }
        public UserDTO Questioner { get; set; }
        public DateTime QuestionTime { get; set; }
        public DateTime LastTime { get; set; }
    }
}
