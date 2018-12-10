using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Qincai.Dtos;
using Qincai.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Qincai.Services.Test
{
    public class UserSeriveTest : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public UserSeriveTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("UserServicesTest")
                .Options;
            _context = new ApplicationDbContext(options);
            _userService = new UserService(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task ExistByIdAsyncTest()
        {
            User user = new User();
            (await _userService.ExistByIdAsync(user.Id)).Should().BeFalse();
            AddUser(user);
            (await _userService.ExistByIdAsync(user.Id)).Should().BeTrue();
        }

        [Fact]
        public async Task ExistByOpenIdAsyncTest()
        {
            User user = new User { WxOpenId = "wxopenid" };
            (await _userService.ExistByOpenIdAsync(user.WxOpenId)).Should().BeFalse();
            AddUser(user);
            (await _userService.ExistByOpenIdAsync(user.WxOpenId)).Should().BeTrue();
        }

        [Fact]
        public async Task GetByIdAsyncTest()
        {
            User user = new User();
            (await _userService.GetByIdAsync(user.Id)).Should().BeNull();
            AddUser(user);
            (await _userService.GetByIdAsync(user.Id)).Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task GetByOpenIdAsyncTest()
        {
            User user = new User { WxOpenId = "wxopenid" };
            (await _userService.GetByOpenIdAsync(user.WxOpenId)).Should().BeNull();
            AddUser(user);
            (await _userService.GetByOpenIdAsync(user.WxOpenId)).Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task CreateAsyncTest()
        {
            var dto = new CreateUserParam
            {
                Name = "Test User",
                AvatarUrl = "https://placehold.it/200x200",
                Role = UserRole.Admin,
                WxOpenId = "wxopenid"
            };
            User userReturn = await _userService.CreateAsync(dto);
            userReturn.Should().BeEquivalentTo(dto);

            User userFromDb = _context.Users.FirstOrDefault();
            userFromDb.Should().BeEquivalentTo(userReturn);
        }

        [Fact]
        public async Task UpdateAsyncTest()
        {
            var dto = new UpdateUserParam
            {
                Name = "new user name",
                AvatarUrl = "https://placehold.it/200x200"
            };
            User user = new User();
            AddUser(user);
            await _userService.UpdateAsync(user.Id, dto);
            User userFromDb = _context.Users.FirstOrDefault();
            userFromDb.Name.Should().Be(dto.Name);
            userFromDb.AvatarUrl.Should().Be(dto.AvatarUrl);
            userFromDb.Should().BeEquivalentTo(user, optons =>
                optons.Excluding(u => u.Name).Excluding(u => u.AvatarUrl));
        }

        private void AddUser(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
        }
    }
}
