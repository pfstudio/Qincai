using Microsoft.EntityFrameworkCore;
using Qincai.Dtos;
using Qincai.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Qincai.Services
{
    /// <summary>
    /// 用户相关服务接口
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 根据Id判断用户是否存在
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户是否存在</returns>
        Task<bool> ExistByIdAsync(Guid userId);
        /// <summary>
        /// 根据OpenId判断用户是否存在
        /// </summary>
        /// <param name="openId">微信用户OpenId</param>
        /// <returns>用户是否存在</returns>
        // TODO: 此处微信逻辑对用户服务有所侵入
        Task<bool> ExistByOpenIdAsync(string openId);
        /// <summary>
        /// 根据Id获取用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户实体</returns>
        Task<User> GetByIdAsync(Guid userId);
        /// <summary>
        /// 根据OpenId获取用户
        /// </summary>
        /// <param name="openId">微信用户OpenId</param>
        /// <returns>用户实体</returns>
        // TODO: 此处微信逻辑对用户服务有所侵入
        Task<User> GetByOpenIdAsync(string openId);
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="dto">创建用户参数</param>
        /// <returns>创建的用户</returns>
        Task<User> CreateAsync(CreateUserParam dto);
    }

    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="context">数据库上下文</param>
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// <see cref="IUserService.ExistByIdAsync(Guid)"/>
        /// </summary>
        public async Task<bool> ExistByIdAsync(Guid userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }

        /// <summary>
        /// <see cref="IUserService.ExistByOpenIdAsync(string)"/>
        /// </summary>
        public async Task<bool> ExistByOpenIdAsync(string openId)
        {
            return await _context.Users.AnyAsync(u => u.WxOpenId == openId);
        }

        /// <summary>
        /// <see cref="IUserService.GetByIdAsync(Guid)"/>
        /// </summary>
        public async Task<User> GetByIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        /// <summary>
        /// <see cref="IUserService.GetByOpenIdAsync(string)"/>
        /// </summary>
        public async Task<User> GetByOpenIdAsync(string openId)
        {
            return await _context.Users.Where(u => u.WxOpenId == openId)
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// <see cref="IUserService.CreateAsync(CreateUserParam)"/>
        /// </summary>
        public async Task<User> CreateAsync(CreateUserParam dto)
        {
            User user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                AvatarUrl = dto.AvatarUrl,
                WxOpenId = dto.WxOpenId,
                Role = dto.Role
            };

            // TODO: 数据库异常处理
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
