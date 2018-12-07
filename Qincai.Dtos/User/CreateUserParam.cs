using System.ComponentModel.DataAnnotations;

namespace Qincai.Dtos
{
    /// <summary>
    /// 创建用户参数
    /// </summary>
    public class CreateUserParam
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 头像Url
        /// </summary>
        public string AvatarUrl { get; set; }
        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string WxOpenId { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "用户角色不为空")]
        public string Role { get; set; }
    }
}
