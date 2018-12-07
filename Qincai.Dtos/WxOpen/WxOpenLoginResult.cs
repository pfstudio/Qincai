namespace Qincai.Dtos
{
    /// <summary>
    /// 登录结果
    /// </summary>
    public class WxOpenLoginResult
    {
        /// <summary>
        /// 用户属性
        /// </summary>
        public UserDto User { get; private set; }
        /// <summary>
        /// 3rd-Session Id
        /// </summary>
        public string SessionId { get; private set; }

        public WxOpenLoginResult(string sessionId, UserDto user=null)
        {
            SessionId = sessionId;
            User = user;
        }
    }
}
