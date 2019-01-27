using Microsoft.Extensions.Options;
using Qincai.Dtos;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Qincai.Services
{
    /// <summary>
    /// Redis存储服务相关接口
    /// </summary>
    public interface IRedisService
    {
        /// <summary>
        /// 获得值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="database">指定数据库(可选)</param>
        /// <returns></returns>
        Task<string> Get(string key, int database = 0);

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="database">指定数据库(可选)</param>
        /// <returns></returns>
        Task<bool> Set(string key, string value, int database = 0);

        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="database">指定数据库(可选)</param>
        /// <returns></returns>
        Task<bool> Delete(string key, int database = 0);
    }

    /// <summary>
    /// Redis存储服务
    /// </summary>
    public class RedisService : IRedisService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly SmsConfig _smsConfig;

        /// <summary>
        /// 实例化Redis
        /// </summary>
        public RedisService(IOptions<SmsConfig> smsConfig)
        {
            _smsConfig = smsConfig.Value;
            _redis = ConnectionMultiplexer.Connect(_smsConfig.Redis.connectingString);
        }

        /// <summary>
        /// <see cref="IRedisService.Delete(string, int)"/>
        /// </summary>
        public async Task<bool> Delete(string key, int database = 0)
        {
            if (string.IsNullOrEmpty(key)) return false;
            try
            {
                await _redis.GetDatabase(database).KeyDeleteAsync(key);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }


        /// <summary>
        /// <see cref="IRedisService.Get(string, int)"/>
        /// </summary>
        public async Task<string> Get(string key, int database = 0)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException();
            return await _redis.GetDatabase(database).StringGetAsync(key);
        }
        /// <summary>
        /// <see cref="IRedisService.Set(string, string, int)"/>
        /// </summary>
        public async Task<bool> Set(string key, string value, int database = 0)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return false;
            try
            {
                await _redis.GetDatabase(database).StringSetAsync(key, value, TimeSpan.FromSeconds(_smsConfig.ExpireTime));

                return true;
            }
            catch (Exception e)
            {
                //TODO: 错误处理
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
