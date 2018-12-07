using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Qincai.Models.Test
{
    public class ModelTest
    {
        public ModelTest()
        {
        }

        [Fact]
        public void QuestionTest()
        {
            Question question = new Question
            {
            };
            Assert.False(question.IsDelete);
        }
    }
}
