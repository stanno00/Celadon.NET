using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DotNetTribes.Services
{
    public class JwtService : IJwtService
    {
        private readonly string SECRET_KEY;

        public JwtService()
        {
            SECRET_KEY = Environment.GetEnvironmentVariable("SECRET_KEY");
        }

        public string CreateToken(string name, string kingdomId)
        {
            var symmetricKey = Encoding.ASCII.GetBytes(SECRET_KEY);
            
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim("Username", name),
                        new Claim("KindomId", kingdomId)
                    }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(securityTokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);

            return token;
        }
        
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