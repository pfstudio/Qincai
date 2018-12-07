namespace Qincai.Dtos
{
    /// <summary>
    /// 图片上传Token
    /// </summary>
    public class ImageUploadToken
    {
        /// <summary>
        /// 上传Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 允许的文件名
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 上传图片的域名
        /// </summary>
        public string UploadDomain { get; set; }
    }
}
