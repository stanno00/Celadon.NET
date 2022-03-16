using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace DotNetTribes.Services
{//Use to get username and kingdomId
    public class AuthService : IAuthService
    {
        public string GetNameFromJwt(string jwt)
        {
            string name = GetClaimsPrincipal(jwt).Claims.First(claim => claim.Type == "Username").Value;
            return name;
        }

        public int GetKingdomIdFromJwt(string jwt)
        {
            var result = GetClaimsPrincipal(jwt).Claims.First(claim => claim.Type == "KingdomId").Value;
            return Convert.ToInt32(result);
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