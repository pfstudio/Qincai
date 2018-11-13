using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qincai.Api.Dtos
{
    public class AuthorizeResult
    {
        public bool Status { get; set; }
        public string Token { get; set; }
        public string ErrMsg { get; set; }

        private AuthorizeResult() { }

        public static AuthorizeResult Success(string Token)
        {
            return new AuthorizeResult
            {
                Status = true,
                Token = Token,
                ErrMsg = ""
            };
        }

        public static AuthorizeResult Fail(string errMsg)
        {
            return new AuthorizeResult
            {
                Status = false,
                Token = null,
                ErrMsg = ""
            };
        }
    }
}
