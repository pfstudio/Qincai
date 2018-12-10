using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Qincai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Qincai.Services.Test
{
    public class AnswerServiceTest : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IAnswerService _answerService;

        public AnswerServiceTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("AnswerServicesTest")
                .Options;
            _context = new ApplicationDbContext(options);
            _answerService = new AnswerService(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetByIdAsyncTest()
        {
            // TODO: 此处外键约束
            Question question = new Question();
            Answer answer = new Answer { Question = question };
            question.Answers.Add(answer);
            _context.Add(question);
            _context.SaveChanges();

            Answer answerFromDb = await _answerService.GetByIdAsync(answer.Id);
            answerFromDb.Should().BeEquivalentTo(answer);
        }

        [Fact]
        public void GetQueryTest()
        {
            Question question = new Question();
            question.Answers.Add(new Answer { Question = question });
            question.Answers.Add(new Answer { Question = question });
            _context.AddRange(question);
            _context.SaveChanges();

            var answersFromDb = _answerService.GetQuery().ToList();
            answersFromDb.Should().BeEquivalentTo(question.Answers, options => options.Excluding(a => a.Question));
        }

        [Fact]
        public async Task DeleteAsyncTest()
        {
            Answer answer = new Answer();
            _context.Add(answer);
            _context.SaveChanges();

            await _answerService.DeleteAsync(answer.Id);
            _context.Answers.FirstOrDefault(a => a.Id == answer.Id).Should().BeNull();

            Answer answerFromDb = _context.Answers.IgnoreQueryFilters().FirstOrDefault(a => a.Id == answer.Id);
            answerFromDb.IsDelete.Should().BeTrue();
            answerFromDb.Should().BeEquivalentTo(answer, options => options.Excluding(a => a.IsDelete));
        }
    }
}
