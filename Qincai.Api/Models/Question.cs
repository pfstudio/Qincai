using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qincai.Api.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Content Content { get; set; }
        //public List<string> Tags { get; set; }
        public List<Answer> Answers { get; set; }
        public User Questioner { get; set; }
        public DateTime QuestionTime { get; set; }
        public DateTime LastTime { get; set; }
        //public QuestionType Type { get; set; }
        //public QuestionStatus Status { get; set; }

        public static Question Create(
            User questioner, string title, Content content)
        {
            return new Question
            {
                Id = Guid.NewGuid(),
                Questioner = questioner,
                Title = title,
                Content = content,
                QuestionTime = DateTime.Now,
                LastTime = DateTime.Now
            };
        }
    }

    public enum QuestionType
    {
        OnlyTeacher,
        Public
    }

    public enum QuestionStatus
    {
        Open,
        Closed
    }
}
