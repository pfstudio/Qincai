using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Qincai.Dtos;
using Qincai.Services;

namespace Qincai.Api.Controllers
{
    /// <summary>
    /// 短信相关API
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SmsController : ControllerBase
    {
        private readonly ISmsService _smsService;
        private readonly IRedisService _redisService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smsService"></param>
        /// <param name="redisService"></param>
        public SmsController(ISmsService smsService, IRedisService redisService)
        {
            _smsService = smsService;
            _redisService = redisService;
        }


        [HttpPost("[Action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> Send([FromForm] SendVerificationCodeParam param)
        {
            //非空判定
            if (param==null || string.IsNullOrEmpty(param.PhoneNumber))
                return BadRequest("手机号码为空");

            //验证手机号码可用性
            if (!_smsService.PhoneIsAvailable(param.PhoneNumber))
                return BadRequest("手机号码不可用");

            //已有验证码则不发送
            if (!string.IsNullOrEmpty(await _redisService.Get(param.PhoneNumber)))
                return BadRequest("重新验证等待中");

            try
            {
                //生成六位随机数字
                string VarificationCode = new Random().Next(0, 999999).ToString().PadLeft(6, '0');

                //存入Redis
                await _redisService.Set(param.PhoneNumber, VarificationCode);

                //发送短信
                if (!_smsService.SendCode(param.PhoneNumber, VarificationCode))
                    return BadRequest("短信发送失败");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("[Action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> Verify([FromForm]VerifyVerificationCodeParam param)
        {
            //非空判定
            if (param==null || string.IsNullOrEmpty(param.PhoneNumber) || string.IsNullOrEmpty(param.VerificationCode))
                return BadRequest("手机号或验证码为空");

            string storageCode = await _redisService.Get(param.PhoneNumber);
            //不存在手机号对应的验证码则返回错误
            if (string.IsNullOrEmpty(storageCode))
                return BadRequest("未获取验证码");

            //检验验证码
            if (storageCode.Equals(param.VerificationCode))
            {
                //过检验后删除该值
                await _redisService.Delete(param.PhoneNumber);
                return true;
            }

            return false;
        }
    }
}