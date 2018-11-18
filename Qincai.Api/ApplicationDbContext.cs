using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        //public DbSet<Image> Images { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var question = modelBuilder.Entity<Question>();
            var answer = modelBuilder.Entity<Answer>();
            var user = modelBuilder.Entity<User>();
            //var image = modelBuilder.Entity<Image>();

            var splitStringConverter = new ValueConverter<List<string>, string>(
                l => string.Join(";", l),
                s => s.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());

            question.Property(q => q.Id)
                .ValueGeneratedNever()
                .IsRequired();
            question.Property(q => q.Title)
                .IsRequired()
                .HasMaxLength(100);
            question.HasOne(q => q.Questioner);
            question.OwnsOne(q => q.Content)
                .Property(c => c.Images)
                .HasConversion(splitStringConverter);

            answer.Property(a => a.Id)
                .ValueGeneratedNever()
                .IsRequired();
            answer.HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .IsRequired();
            answer.HasOne(a => a.Answerer);
            answer.HasOne(a => a.RefAnswer);
            answer.OwnsOne(a => a.Content)
                .Property(c => c.Images)
                .HasConversion(splitStringConverter);

            user.Property(u => u.Id)
                .ValueGeneratedNever()
                .IsRequired();
            user.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(20);

            //image.Property(i => i.Id)
            //    .ValueGeneratedNever()
            //    .IsRequired();
            //image.Property(i => i.SoureUrl)
            //    .IsRequired();
            //image.Property(i => i.UploaderId)
            //    .IsRequired();
        }
    }
}
