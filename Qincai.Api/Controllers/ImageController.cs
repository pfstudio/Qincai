using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Qincai.Api.Dtos;
using Qincai.Api.Extensions;
using Qincai.Api.Models;
using Qincai.Api.Utils;
using Qiniu.Storage;
using Qiniu.Util;

namespace Qincai.Api.Controllers
{
    /// <summary>
    /// 图片管理相关API
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    //[Authorize]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly QiniuConfig _qiniuConfig;

        public ImageController(ApplicationDbContext context, IMapper mapper, IOptions<QiniuConfig> qiniuConfig)
        {
            _context = context;
            _mapper = mapper;
            _qiniuConfig = qiniuConfig.Value;
        }

        /// <summary>
        /// 返回图片上传Token
        /// </summary>
        /// <param name="fileExt">文件后缀(格式)</param>
        /// <returns></returns>
        [HttpPost("getToken")]
        public async Task<ActionResult<ImgTokenDto>> GetToken([FromBody] string fileExt)
        {
            //TODO 检测文件后缀是否合法

            //User thisUser = _context.Users.FirstOrDefault(u => u.Id == User.GetUserId());

            User thisUser =  _context.Users.OrderBy(x => Guid.NewGuid()).First();

            //未获得对应上传者
            if (thisUser == null)
            {
                return NotFound("No Authotize To Upload");
            }

            ImgTokenDto Token = CreateToken(thisUser.Id, fileExt);

            //记录图片信息
            if (!await StroageImgInfo(thisUser.Id,Token.Key)) return BadRequest();

            return Token;
        }



        /// <summary>
        /// 生成Token
        /// </summary>
        /// <returns></returns>
        private ImgTokenDto CreateToken(Guid userId, string fileExt)
        {
            Mac mac = new Mac(_qiniuConfig.ACCESS_KEY, _qiniuConfig.SECRET_KEY);
            Auth auth = new Auth(mac);
            PutPolicy putPolicy = new PutPolicy()
            {
                //使用后缀
                Scope = _qiniuConfig.ImageBucket.Name + ":" + userId.ToString() + "/" + DateTime.Now.ToString("yyyyMMddhhmmss") + "-" + new Random().Next(100, 900).ToString() + "." + fileExt
            };
            putPolicy.SetExpires(120);
            //限制图片大小
            putPolicy.FsizeLimit = 10 * 1024 * 1024;
            //七牛云上传成功后对服务器的回调
            //putPolicy.CallbackUrl ="";
            string jstr = putPolicy.ToJsonString();

            //返回生成的token模型
            return new ImgTokenDto()
            {
                Token = auth.CreateUploadToken(jstr),
                Key = putPolicy.Scope.Substring(putPolicy.Scope.IndexOf(":")+1),
                UploadDomain=_qiniuConfig.ImageBucket.UploadDomain
            };
        }

        /// <summary>
        /// 记录图片信息
        /// </summary>
        /// <param name="userId">上传者Id</param>
        /// <param name="sourceUrl">图片相对路径</param>
        /// <returns></returns>
        private async Task<bool> StroageImgInfo(Guid userId,string sourceUrl)
        {
            //因为限定了Scope所以可以推断出真实的图片地址
            string thisUrl = _qiniuConfig.ImageBucket.DownloadDomain + "/"+ _qiniuConfig.ImageBucket.Name + "/" + sourceUrl;
            try
            {
                var thisImage = Image.Create(userId, thisUrl);
                await _context.Images.AddAsync(thisImage);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            
            return true;
        }
    }
}