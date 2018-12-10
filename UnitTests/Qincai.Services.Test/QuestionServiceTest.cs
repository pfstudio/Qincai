using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Qincai.Dtos;
using Qincai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Qincai.Services.Test
{
    public class QuestionServiceTest: IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IQuestionService _questionService;

        public QuestionServiceTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("QuestionServicesTest")
                .Options;
            _context = new ApplicationDbContext(options);
            _questionService = new QuestionService(_context);

        }

        // 在每一个测试实例之后被调用
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetByIdAsyncTest()
        {
            Question question = new Question();
            _context.Add(question);
            _context.SaveChanges();

            Question questionFromDb = await _questionService.GetByIdAsync(question.Id);
            questionFromDb.Should().Be(question);
        }

        [Fact]
        public void GetQueryTest()
        {
            List<Question> questions = new List<Question>
            {
                new Question(), new Question()
            };
            _context.AddRange(questions);
            _context.SaveChanges();

            var questionsFromDb = _questionService.GetQuery().ToList();
            questionsFromDb.Should().BeEquivalentTo(questions);
        }

        [Fact]
        public void GetAnswersQueryTest()
        {
            Question question = new Question();
            question.Answers.Add(new Answer() { Question = question });
            question.Answers.Add(new Answer() { Question = question });
            _context.Add(question);
            _context.SaveChanges();

            var answersFromDb = _questionService.GetAnswersQuery(question.Id).ToList();
            answersFromDb.Should().BeEquivalentTo(question.Answers, options => options.Excluding(a => a.Question));
        }

        [Fact]
        public async Task ExistAsync()
        {
            Question question = new Question();
            (await _questionService.ExistAsync(question.Id)).Should().BeFalse();

            _context.Add(question);
            _context.SaveChanges();

            (await _questionService.ExistAsync(question.Id)).Should().BeTrue();
        }

        [Fact]
        public async Task CreateQuestionTest()
        {
            User questioner = new User { Name = "Tset user" };
            _context.Add(questioner);
            _context.SaveChanges();
            var dto = new CreateQuestionParam
            {
                Title = "Title",
                Text = "text content",
                Category = "Test",
                Images = new List<string>
                {
                    "https://placehold.it/200x200"
                }
            };
            // 测试返回的Question对象
            Question questionReturn = await _questionService.CreateAsync(questioner, dto);
            questionReturn.Title.Should().Be(dto.Title);
            questionReturn.Category.Should().Be(dto.Category);
            questionReturn.Content.Text.Should().Be(dto.Text);
            questionReturn.Content.Images.Should().BeEquivalentTo(dto.Images);
            questionReturn.Questioner.Should().BeEquivalentTo(questioner);
            // 测试时间应该不超过1分钟
            questionReturn.QuestionTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
            questionReturn.LastTime.Should().BeCloseTo(questionReturn.QuestionTime);
            questionReturn.IsDelete.Should().BeFalse();
            questionReturn.Owner.Should().Be(questionReturn.Questioner);
            questionReturn.Answers.Should().BeEmpty();

            // 测试数据库中的Question对象
            Question questionFromDb = _context.Questions.FirstOrDefault();
            questionFromDb.Should().BeEquivalentTo(questionReturn);
        }

        [Fact]
        public async Task ReplyAsyncTest()
        {
            User questioner = new User { Name = "Questioner" };
            User answerer = new User { Name = "Answerer" };
            Question question = new Question
            {
                Questioner = questioner
            };
            Answer answer = new Answer{ Answerer = answerer };
            question.Answers.Add(answer);
            _context.Add(question);
            _context.SaveChanges();

            var dto = new ReplyQuestionParam
            {
                Text = "answer text",
                Images = new List<string> { "https://placehold.it/200x200" },
                RefAnswerId = answer.Id
            };
            // 测试返回的Answer对象
            Answer answerReturn = await _questionService.ReplyAsync(question.Id, answerer, dto);
            answerReturn.Content.Text.Should().Be(dto.Text);
            answerReturn.Content.Images.Should().BeEquivalentTo(dto.Images);
            answerReturn.RefAnswer.Id.Should().Be(dto.RefAnswerId.Value);
            answerReturn.Answerer.Should().Be(answerer);
            answerReturn.AnswerTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
            answerReturn.IsDelete.Should().BeFalse();

            // 测试数据库中的Answer对象
            Answer answerFromDb = _context.Answers.FirstOrDefault(a => a.Id == answerReturn.Id);
            answerFromDb.Should().BeEquivalentTo(answerReturn, options => options.ExcludingNestedObjects());
        }

        [Fact]
        public async Task DeleteAsync()
        {
            Question question = new Question();
            _context.Add(question);
            _context.SaveChanges();

            await _questionService.DeleteAsync(question.Id);
            _context.Questions.FirstOrDefault(q => q.Id == question.Id).Should().BeNull("软删除默认被过滤");

            Question questionFromDb = _context.Questions.IgnoreQueryFilters().FirstOrDefault(q => q.Id == question.Id);
            questionFromDb.IsDelete.Should().BeTrue("软删除IsDelete属性应被设置为True");
            questionFromDb.Should().BeEquivalentTo(question, options => options.Excluding(q => q.IsDelete), "软删除其余属性不变");
        }
    }
}
