﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Qincai.Dtos
{
    /// <summary>
    /// 回答问题参数
    /// </summary>
    public class ReplyQuestionParam
    {
        /// <summary>
        /// 回答内容
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "回答内容不为空")]
        public string Text { get; set; }
        /// <summary>
        /// 回答中包含的图片
        /// </summary>
        [MaxLength(3, ErrorMessage = "回答图片不超过3幅")]
        public List<string> Images { get; set; }
        /// <summary>
        /// 引用的回答
        /// </summary>
        public Guid? RefAnswerId { get; set; }
    }
}
