namespace Qincai.Dtos
{
    /// <summary>
    /// 短信相关配置
    /// </summary>
    public class SmsConfig
    {
        /// <summary>
        /// 短信SDK配置
        /// </summary>
        public SMS SMS { get; set; }

        /// <summary>
        /// Redis配置
        /// </summary>
        public Redis Redis { get; set; }

        /// <summary>
        /// 过期时间/秒
        /// </summary>
        public int ExpireTime { get; set; }
    }

    /// <summary>
    /// 短信SDK配置
    /// </summary>
    public class SMS
    {
        /// <summary>
        /// skdappid
        /// </summary>
        public int sdkappid { get; set; }
        /// <summary>
        /// appkey
        /// </summary>
        public string appkey { get; set; }
        /// <summary>
        /// nationCode
        /// </summary>
        public string nationCode { get; set; }
        /// <summary>
        /// templateId
        /// </summary>
        public int templateId { get; set; }
        /// <summary>
        /// smsSign
        /// </summary>
        public string smsSign { get; set; }
    }

    /// <summary>
    /// Redis配置
    /// </summary>
    public class Redis
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string connectingString { get; set; }
        /// <summary>
        /// 默认的数据库
        /// </summary>
        public int defaultDB { get; set; }
    }
}
