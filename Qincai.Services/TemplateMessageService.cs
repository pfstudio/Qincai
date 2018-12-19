using Microsoft.Extensions.Options;
using Qincai.Dtos;
using Qincai.Models;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Template;
using System.Threading.Tasks;

namespace Qincai.Services
{
    /// <summary>
    /// 模板消息服务接口
    /// </summary>
    public interface ITemplateMessageService
    {
        /// <summary>
        /// 问题回复通知
        /// </summary>
        /// <param name="formId">表单Id</param>
        /// <param name="answer">回答</param>
        /// <param name="remark">备注</param>
        /// <param name="templateId">模板Id</param>
        Task SendMessageAsync_ReplyQuestion(string formId, Answer answer, string remark=null, string templateId="XaKQGRK0mw8MalQkBDl2uZ0EytQy7A6Ze7opsY7lUqI");
    }

    /// <summary>
    /// 模板消息服务
    /// </summary>
    public class TemplateMessageService : ITemplateMessageService
    {
        private readonly WxOpenConfig _wxOpenConfig;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="wxOpenConfig">微信配置</param>
        public TemplateMessageService(IOptions<WxOpenConfig> wxOpenConfig)
        {
            _wxOpenConfig = wxOpenConfig.Value;
        }

        /// <summary>
        /// <see cref="ITemplateMessageService.SendMessageAsync_ReplyQuestion(string, Answer, string, string)"/>
        /// </summary>
        public async Task SendMessageAsync_ReplyQuestion(string formId, Answer answer, string remark, string templateId)
        {
            // 目标用户OpenId
            string openId = answer.Question.Questioner.WxOpenId;
            // 截取回复内容
            string subAnswerText = answer.Content.Text.Length > 30 ?
                answer.Content.Text.Substring(0, 30) + "..." : answer.Content.Text;

            // 模板格式
            // {{first.DATA}}
            // 提问主题: {{keyword1.DATA}}
            // 回复内容: {{keyword2.DATA}}
            // 回复人: {{keyword3.DATA}}
            // 回答时间: {{keyword4.DATA}}
            // {{remark.DATA}}
            var data = new
            {
                first    = new TemplateDataItem("您的问题有新的回复"),
                keyword1 = new TemplateDataItem(answer.Question.Title),
                keyword2 = new TemplateDataItem(subAnswerText),
                keyword3 = new TemplateDataItem(answer.Answerer.Name),
                keyword4 = new TemplateDataItem(answer.AnswerTime.ToString()),
                remark   = new TemplateDataItem(remark)
            };

            // 发送模板消息
            var result = await TemplateApi.SendTemplateMessageAsync(_wxOpenConfig.AppId, openId,
                templateId, data, formId, page: $"pages/detail/detail?questionId={answer.Question.Id}");

            // 当发送模板消息失败时，抛出异常
            if (result.errcode != Senparc.Weixin.ReturnCode.请求成功)
            {
                throw new WeixinException(result.errmsg);
            }
        }
    }
}