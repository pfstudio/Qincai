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
            //User thisUser = _context.Users.FirstOrDefault(u => u.Id == User.GetUserId());

            User thisUser =  _context.Users.OrderBy(x => Guid.NewGuid()).First();

            //未获得对应上传者
            if (thisUser == null)
            {
                return NotFound("No Authotize To Upload");
            }

            ImgTokenDto Token = CreateToken(thisUser.Id.ToString(), fileExt);

            return Token;
        }



        /// <summary>
        /// 生成Token
        /// </summary>
        /// <returns></returns>
        private ImgTokenDto CreateToken(string userId, string fileExt)
        {
            Mac mac = new Mac(_qiniuConfig.ACCESS_KEY, _qiniuConfig.SECRET_KEY);
            Auth auth = new Auth(mac);
            PutPolicy putPolicy = new PutPolicy()
            {
                //使用后缀
                Scope = _qiniuConfig.ImageBucket.Name + ":" + userId + "/" + DateTime.Now.ToString("yyyyMMddhhmmss") + "-" + new Random().Next(100, 900).ToString() + "." + fileExt
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
                Scope = putPolicy.Scope,
                UploadDomain=_qiniuConfig.ImageBucket.UploadDomain
            };
        }
    }
}