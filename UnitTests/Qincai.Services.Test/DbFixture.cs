using Microsoft.EntityFrameworkCore;
using Qincai.Models;
using System;

namespace Qincai.Services.Test
{
    public class DbFixture : IDisposable
    {
        public ApplicationDbContext Context { get; private set; }

        public DbFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("QincaiServicesTest")
                .Options;
            Context = new ApplicationDbContext(options);
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
        }
    }
}
