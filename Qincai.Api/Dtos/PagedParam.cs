using System.ComponentModel.DataAnnotations;

namespace Qincai.Api.Dtos
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public interface IPagedParam
    {
        /// <summary>
        /// 当前页数
        /// </summary>
        int Page { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        int PageSize { get; set; }
    }

    /// <summary>
    /// 问题的分页参数
    /// </summary>
    public class QuestionPagedParam: IPagedParam
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
    }

    /// <summary>
    /// 回答的分页参数
    /// </summary>
    public class AnswerPagedParam : IPagedParam
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
    }
}
