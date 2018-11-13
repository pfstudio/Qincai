using Microsoft.EntityFrameworkCore;
using Qincai.Api.Dtos;
using Qincai.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qincai.Api.Services
{
    public interface IUserService
    {
        Task<bool> ExistByIdAsync(Guid userId);
        Task<bool> ExistByOpenIdAsync(string openId);
        Task<User> GetByIdAsync(Guid userId);
        Task<User> GetByOpenIdAsync(string openId);
        Task<User> CreateAsync(CreateUser dto);
    }

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateAsync(CreateUser dto)
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

        public async Task<bool> ExistByIdAsync(Guid userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }

        public async Task<bool> ExistByOpenIdAsync(string openId)
        {
            return await _context.Users.AnyAsync(u => u.WxOpenId == openId);
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> GetByOpenIdAsync(string openId)
        {
            return await _context.Users.Where(u => u.WxOpenId == openId)
                .SingleOrDefaultAsync();
        }
    }
}
