using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DotNetTribes.Services
{//Use to get username and kingdomId
    public class AuthService : IAuthService
    {
        public string GetNameFromJwt(string jwt)
        {
            string name;
            if (!jwt.Contains("Bearer "))
            {
                name = GetClaimsPrincipal(jwt).Claims.First(claim => claim.Type == "Username").Value;
            
                return name;
            }
            string jwtClean = jwt.Replace("Bearer ", "");
            name = GetClaimsPrincipal(jwtClean).Claims.First(claim => claim.Type == "Username").Value;
            
            return name;
        }

        public string GetKingdomIdFromJwt(string jwt)
        {
            if (!jwt.Contains("Bearer "))
            {
                return GetClaimsPrincipal(jwt).Claims.First(claim => claim.Type == "KingdomId").Value;
            }
            string jwtClean = jwt.Replace("Bearer ", "");
            return GetClaimsPrincipal(jwtClean).Claims.First(claim => claim.Type == "KingdomId").Value;
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