namespace Qincai.Api.Dtos
{
    /// <summary>
    /// 认证结果
    /// </summary>
    public class AuthorizeResult
    {
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// Jwt Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }

        private AuthorizeResult() { }

        /// <summary>
        /// 认证成功
        /// </summary>
        /// <param name="Token">Jwt Token</param>
        public static AuthorizeResult Success(string Token)
        {
            return new AuthorizeResult
            {
                Status = true,
                Token = Token,
                ErrMsg = ""
            };
        }

        /// <summary>
        /// 认证失败
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        public static AuthorizeResult Fail(string errMsg)
        {
            return new AuthorizeResult
            {
                Status = false,
                Token = null,
                ErrMsg = ""
            };
        }
    }
}
