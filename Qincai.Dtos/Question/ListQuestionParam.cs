﻿using System.ComponentModel.DataAnnotations;

namespace Qincai.Dtos
{
    /// <summary>
    /// 问题列表参数
    /// </summary>
    public class ListQuestionParam : IPagedParam, ISortedParam
    {
        /// <summary>
        /// 当前页数
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "页码需大于0")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// 每页数量
        /// </summary>
        [Range(1, 50, ErrorMessage = "每页数量不大于50")]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 排序字段(*QuestionTime|LastTime)
        /// </summary>
        [RegularExpression("^(QuestionTime|LastTime)$", ErrorMessage = "排序字段错误")]
        public string OrderBy { get; set; } = "QuestionTime";

        /// <summary>
        /// 是否降序
        /// </summary>
        public bool Descending { get; set; } = true;
    }
}
