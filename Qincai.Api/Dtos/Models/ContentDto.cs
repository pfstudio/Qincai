using System.Collections.Generic;

namespace Qincai.Api.Dtos
{
    /// <summary>
    /// 内容Dto
    /// </summary>
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
