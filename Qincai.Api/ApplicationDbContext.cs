using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Qincai.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Qincai.Api
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// 问题表
        /// </summary>
        public DbSet<Question> Questions { get; set; }
        /// <summary>
        /// 回答表
        /// </summary>
        public DbSet<Answer> Answers { get; set; }
        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options">数据库参数</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
        }

        /// <summary>
        /// 配置数据库架构
        /// </summary>
        /// <param name="modelBuilder"><see cref="ModelBuilder"/></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var question = modelBuilder.Entity<Question>();
            var answer = modelBuilder.Entity<Answer>();
            var user = modelBuilder.Entity<User>();

            // 将字符串列表用;拼接存储
            var splitStringConverter = new ValueConverter<List<string>, string>(
                l => string.Join(";", l),
                s => s.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());

            // 配置问题表
            question.Property(q => q.Id)
                // TODO: 生产环境中需由数据库生成
                .ValueGeneratedNever()
                .IsRequired();
            question.Property(q => q.Title)
                .IsRequired()
                .HasMaxLength(100);
            question.HasOne(q => q.Questioner);
            question.OwnsOne(q => q.Content)
                .Property(c => c.Images)
                .HasConversion(splitStringConverter);

            // 配置回答表
            answer.Property(a => a.Id)
                // TODO: 生产环境中需由数据库生成
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

            // 配置用户表
            user.Property(u => u.Id)
                // TODO: 生产环境中需由数据库生成
                .ValueGeneratedNever()
                .IsRequired();
            user.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}
