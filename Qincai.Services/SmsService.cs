using Microsoft.Extensions.Options;
using qcloudsms_csharp;
using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;
using Qincai.Dtos;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Qincai.Services
{
    /// <summary>
    /// 短信相关服务接口
    /// </summary>
    public interface ISmsService
    {
        /// <summary>
        /// 验证用户手机是否可用
        /// </summary>
        /// <param name="phone">待验证手机号</param>
        /// <returns></returns>
        bool PhoneIsAvailable(string phone);
        /// <summary>
        /// 发送6位随机验证码
        /// </summary>
        /// <param name="phone">用户手机号</param>
        /// <param name="code">验证码</param>
        /// <returns>发送状态</returns>
        bool SendCode(string phone,string code);
    }

    /// <summary>
    /// 短信服务
    /// </summary>
    public class SmsService : ISmsService
    {
        private readonly SmsConfig _smsConfig;
        private readonly SmsSingleSender _smsSingleSender;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="Config">Sms配置文件</param>
        public SmsService(IOptions<SmsConfig> Config)
        {
            _smsConfig = Config.Value;
            _smsSingleSender = new SmsSingleSender(_smsConfig.SMS.sdkappid, _smsConfig.SMS.appkey);
        }

        /// <summary>
        /// <see cref="ISmsService.PhoneIsAvailable(string)"/>
        /// </summary>
        public bool PhoneIsAvailable(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return false;
            return Regex.IsMatch(phone, @"^(13|14|15|16|18|19|17)\d{9}$");
        }

        /// <summary>
        /// <see cref="ISmsService.SendCode(string,string)"/>
        /// </summary>
        public bool SendCode(string phone,string code)
        {
            if (string.IsNullOrEmpty(phone)) return false;
            //手机号不可用则不发送
            if (!PhoneIsAvailable(phone)) return false;

            //TODO: 错误处理
            try
            {
                // 发送单条短信
                SmsSingleSenderResult singleResult = _smsSingleSender.sendWithParam(_smsConfig.SMS.nationCode, phone,
                    _smsConfig.SMS.templateId, new[] { code, _smsConfig.ExpireTime.ToString() }, _smsConfig.SMS.smsSign, "", "");

                if (singleResult.result != 0) return false;

                return true;
            }
            catch (JSONException e)
            {
                Console.WriteLine(e);
            }
            catch (HTTPException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }
    }
}
