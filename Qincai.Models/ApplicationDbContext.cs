using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Qincai.Models
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
            // 将字符串列表用;拼接存储
            var splitStringConverter = new ValueConverter<List<string>, string>(
                l => string.Join(";", l),
                s => s.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());

            // 为软删除启用全局过滤
            Expression<Func<ISoftDelete, bool>> softDeleteFilter = e => e.IsDelete != true;
            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                if (type.ClrType.GetInterfaces().Any(i => i == typeof(ISoftDelete)))
                {
                    // 创建实体类型的参数
                    var param = Expression.Parameter(type.ClrType, "e");
                    // 调用softDeleteFilter
                    var call = Expression.Invoke(softDeleteFilter, param);
                    // 包装为LambdaExpression
                    var lambda = Expression.Lambda(call, param);
                    modelBuilder.Entity(type.ClrType).HasQueryFilter(lambda);
                }
            }

            var question = modelBuilder.Entity<Question>();
            var answer = modelBuilder.Entity<Answer>();
            var user = modelBuilder.Entity<User>();

            // 配置问题表
            // 问题标题最长为100
            question.Property(q => q.Title)
                .IsRequired()
                .HasMaxLength(100);
            question.HasOne(q => q.Questioner);
            question.OwnsOne(q => q.Content)
                .Property(c => c.Images)
                .HasConversion(splitStringConverter);

            // 配置回答表
            answer.HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .IsRequired();
            answer.HasOne(a => a.Answerer);
            answer.HasOne(a => a.RefAnswer);
            answer.OwnsOne(a => a.Content)
                .Property(c => c.Images)
                .HasConversion(splitStringConverter);

            // 配置用户表
            user.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(20);
            user.Property(u => u.Role)
                .IsRequired();
            // 微信OpenId 不能重复
            user.HasIndex(u => u.WxOpenId)
                .IsUnique();
        }
    }
}
