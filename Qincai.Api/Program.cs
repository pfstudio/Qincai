using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Qincai.Api
{
    /// <summary>
    /// 入口类
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 入口函数
        /// </summary>
        /// <param name="args">启动参数</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 创建默认的WebHost
        /// </summary>
        /// <param name="args">启动参数</param>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .UseStartup<Startup>();
    }
}
