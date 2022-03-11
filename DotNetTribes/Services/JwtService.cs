using System;
using System.IdentityModel.Tokens.Jwt;
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
                    {//here are the information that the token stores and their names
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
    }
}