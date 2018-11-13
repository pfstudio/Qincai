namespace Qincai.Api.Dtos
{
    /// <summary>
    /// 登录结果
    /// </summary>
    public class WxOpenLoginResult
    {
        /// <summary>
        /// 登录状态
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
        /// <summary>
        /// 3rd-Session Id
        /// </summary>
        public string SessionId { get; private set; }

        private WxOpenLoginResult() { }

        /// <summary>
        /// 登录成功
        /// </summary>
        /// <param name="sessionId"><see cref="SessionId"/></param>
        /// <param name="user"><see cref="User"/></param>
        /// <returns><see cref="WxOpenLoginResult"/></returns>
        public static WxOpenLoginResult Success(string sessionId, UserDto user)
        {
            return new WxOpenLoginResult
            {
                Status = true,
                SessionId = sessionId,
                User = user,
                ErrMsg = ""
            };
        }

        /// <summary>
        /// 用户未注册
        /// </summary>
        /// <param name="sessionid"><see cref="SessionId"/></param>
        /// <returns><see cref="WxOpenLoginResult"/></returns>
        public static WxOpenLoginResult UnRegister(string sessionid)
        {
            return new WxOpenLoginResult
            {
                Status = true,
                SessionId = sessionid,
                User = null,
                ErrMsg = "用户未注册"
            };
        }

        /// <summary>
        /// 登录失败
        /// </summary>
        /// <param name="errMsg"><see cref="ErrMsg"/></param>
        /// <returns><see cref="WxOpenLoginResult"/></returns>
        public static WxOpenLoginResult Fail(string errMsg)
        {
            return new WxOpenLoginResult
            {
                Status = true,
                SessionId = null,
                User = null,
                ErrMsg = errMsg
            };
        }
    }
}
