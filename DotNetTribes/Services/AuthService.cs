using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DotNetTribes.Services
{
    public class AuthService : IAuthService
    {
        public string GetNameFromJwt(string jwt)
        {
            string name = GetClaimsPrincipal(jwt).Claims.First(claim => claim.Type == "username").Value;
            
            return name;
        }

        public string GetKingdomIdFromJwt(string jwt)
        {
            return GetClaimsPrincipal(jwt).Claims.First(claim => claim.Type == "kingdomId").Value;
        }
        
        private JwtSecurityToken GetClaimsPrincipal(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtToken);
            var tokenS = jsonToken as JwtSecurityToken;
            
            return tokenS;
        }
    }
}