using System;

namespace Qincai.Api.Dtos
{
    /// <summary>
    /// 用户Dto
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string AvatarUrl { get; set; }
    }
}
