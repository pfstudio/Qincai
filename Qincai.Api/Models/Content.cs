using System.Collections.Generic;

namespace Qincai.Models
{
    /// <summary>
    /// 内容对象
    /// </summary>
    public class Content
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 图片url
        /// </summary>
        public List<string> Images { get; set; }
    }
}
