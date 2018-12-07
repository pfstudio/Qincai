namespace Qincai.Dtos
{
    /// <summary>
    /// JWT配置参数
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 颁发者
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 接受者
        /// </summary>
        public string Audience { get; set; }
    }
}
