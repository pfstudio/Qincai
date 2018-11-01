using Microsoft.EntityFrameworkCore;
using Qincai.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qincai.Api
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var question = modelBuilder.Entity<Question>();
            var answer = modelBuilder.Entity<Answer>();
            var user = modelBuilder.Entity<User>();

            question.Property(q => q.Id)
                .ValueGeneratedNever();
            question.Property(q => q.Title)
                .IsRequired()
                .HasMaxLength(100);
            question.HasOne(q => q.Questioner);
            question.OwnsOne(q => q.Content);

            answer.Property(a => a.Id)
                .ValueGeneratedNever();
            answer.HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .IsRequired();
            answer.HasOne(a => a.Answerer);
            answer.OwnsOne(a => a.Content);

            user.Property(u => u.Id)
                .ValueGeneratedNever();
        }
    }
}
