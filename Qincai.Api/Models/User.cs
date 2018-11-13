using System;

namespace Qincai.Api.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Role { get; internal set; }
        public string AvatarUrl { get; set; }
        public string WxOpenId { get; set; }
    }
}
