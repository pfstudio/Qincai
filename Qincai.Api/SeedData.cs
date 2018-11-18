using Microsoft.Extensions.DependencyInjection;
using Qincai.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Qincai.Api
{
    public static class SeedData
    {
        public static void InitDatabase(IServiceProvider services)
        {
            var dbContent = services.GetRequiredService<ApplicationDbContext>();

            if (dbContent.Users.Any())
            {
                return;
            }

            User u1 = new User
            {
                Id = Guid.Parse("75F0C624-FF7A-4E43-9A47-85C1040594F5"),
                Name = "某不愿意透露姓名的咕咕咕",
                Role = "用户"
            };

            User u2 = new User
            {
                Id = Guid.Parse("35802303-5E67-4C09-8B51-5A4002E16C01"),
                Name = "CSS鬼才",
                Role = "用户"
            };

            User u3 = new User
            {
                Id = Guid.Parse("91A224B1-090A-4735-9ECF-882BE8D7536D"),
                Name = "老师很忙",
                Role = "用户"
            };

            Question q1 = new Question
            {
                Id = Guid.Empty,
                Title = "信息论作业有毒",
                Content = new Content
                {
                    Text = "信息论作业到底是啥啊"
                },
                Questioner = u1,
                QuestionTime = new DateTime(2018, 10, 30, 21, 01, 57),
                LastTime = new DateTime(2018, 11, 1, 10, 01, 00),
                Answers = new List<Answer>
                {
                    new Answer
                    {
                        Id = Guid.Parse("38209AD9-AB44-4929-B8D7-089DCDA3C8FA"),
                        Answerer = u2,
                        Content = new Content
                        {
                            Text = "巧了，我也不知道"
                        },
                        AnswerTime = new DateTime(2018, 11, 1, 10, 01, 00)
                    }
                }
            };

            Question q2 = new Question
            {
                Id = Guid.Parse("21E51167-11A5-4D22-B786-18AEE101846B"),
                Title = "Too young, Too simple",
                Content = new Content
                {
                    Text = "你们呐，还是要提高自己的知识水平！"
                },
                Questioner = u3,
                QuestionTime = DateTime.Now,
                LastTime = DateTime.Now
            };

            dbContent.AddRange(u1, u2, u3);
            dbContent.AddRange(q1, q2);

            dbContent.SaveChanges();
        }
    }
}
