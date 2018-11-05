namespace Qincai.Api.Dtos
{
    /// <summary>
    /// 注册结果
    /// </summary>
    public class RegisterResult
    {
        /// <summary>
        /// 注册状态
        /// </summary>
        public bool Status { get; private set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; private set; }
        /// <summary>
        /// 用户属性
        /// </summary>
        public UserDto User { get; private set; }

        private RegisterResult() { }

        /// <summary>
        /// 创建新的注册结果
        /// </summary>
        /// <param name="user">用户属性</param>
        /// <returns><see cref="RegisterResult"/></returns>
        public static RegisterResult Create(UserDto user)
        {
            return new RegisterResult
            {
                Status = true,
                ErrMsg = "",
                User = user
            };
        }
    }
}
