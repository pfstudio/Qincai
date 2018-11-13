using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qincai.Api.Dtos
{
    public class CreateUser
    {
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public string WxOpenId { get; set; }
        public string Role { get; set; }
    }
}
