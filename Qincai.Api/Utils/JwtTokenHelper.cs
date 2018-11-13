using Microsoft.IdentityModel.Tokens;
using Qincai.Api.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Qincai.Api.Utils
{
    public static class JwtTokenHelper
    {
        public static string Create(JwtConfig jwtConfig, User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var subject = new ClaimsIdentity(claims);
            var credentials = new SigningCredentials(
                jwtConfig.Key, SecurityAlgorithms.HmacSha256Signature);
            var token = tokenHandler.CreateJwtSecurityToken(
                jwtConfig.Issuer, jwtConfig.Audience, subject,
                expires: DateTime.Now.AddMinutes(2), issuedAt: DateTime.Now,
                signingCredentials:credentials);

            return tokenHandler.WriteToken(token);
        }
    }
}
