using System.Collections.Generic;

namespace Qincai.Api.Dtos
{
    public class ContentDto
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 图片内容
        /// </summary>
        public List<string> Images { get; set; }
    }
}
