using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DotNetTribes.Services
{
    public class JWTService : IAuthService
    {
        public string SecretKey { get; set; }

        public JWTService(string secretKey)
        {
            SecretKey = secretKey;
        }

        public string GenerateToken(IAuthContainerService model)
        {
            if (model == null || model.Claims == null || model.Claims.Length == 0)
            {
                throw new ArgumentException("Arguments to create token are not valid");
            }

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(model.Claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(model.ExpireMinutes)),
                SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(), model.SecurityAlgorithm)
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            string token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return token;
        }

        public string GetNameFromJwt(string Jwt)
        {
            var name = GetClaimsPrincipal(Jwt)?.FindFirst("username")?.Value;

            return name;
        }

        public string GetKingdomIdFromJwt(string Jwt)
        {
            return GetClaimsPrincipal(Jwt)?.FindFirst("kingdomId")?.Value;
        }
        
        private ClaimsPrincipal GetClaimsPrincipal(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;
        
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();
        
            validationParameters.ValidateLifetime = true;
        
            validationParameters.IssuerSigningKey =
                new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("TW9zaGVFcmV6UHJpdmF0ZUtleQ==")); //will get from server
        
            ClaimsPrincipal principal =
                new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            var name = principal?.FindFirst("username")?.Value;
            
            return principal;
        }
        
        private SecurityKey GetSymmetricSecurityKey()
        {
            byte[] symmetricKey = Convert.FromBase64String(SecretKey);
            return new SymmetricSecurityKey(symmetricKey);
        }
    }
}