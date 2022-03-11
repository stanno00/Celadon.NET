using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DotNetTribes.DTOs;
using DotNetTribes.Exceptions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DotNetTribes.Services
{
    public class AuthService : IAuthService
    {
        
        private readonly ApplicationContext _applicationContext;

        public AuthService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public LoginResponseDto Login(LoginRequestDto usernamePasswordDto)
        {
            if (usernamePasswordDto == null)
            {
                throw new LoginException("All fields are required.");
            }

            if (string.IsNullOrEmpty(usernamePasswordDto.Password))
            {
                throw new LoginException("Password is required.");
            }
            
            if (string.IsNullOrEmpty(usernamePasswordDto.Username))
            {
                throw new LoginException("Username is required.");
            }

            isUsernameAndPasswordCorrect(usernamePasswordDto);
            
            
            var user = _applicationContext.Users.SingleOrDefault(u => u.Username == usernamePasswordDto.Username);

            JwtService jwtService = new JwtService();
            
            string token = jwtService.CreateToken(user.Username, user.KingdomId.ToString());
            
            
            return new LoginResponseDto()
            {
                Status = "ok",
                Token = token
            };
        }
        public string getNameFromJwt(string jwt)
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

        public string getKingdomIdFromJwt(string jwt)
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
        
        private void isUsernameAndPasswordCorrect(LoginRequestDto usernamePasswordDto)
        {
            var user = _applicationContext.Users.SingleOrDefault(u => u.Username == usernamePasswordDto.Username);
            if (user == null)
            {
                throw new LoginException("Username or password is incorrect.");
            }
            bool verified = BCrypt.Net.BCrypt.Verify(usernamePasswordDto.Password, user.HashedPassword);
            if (verified == false)
            {
                throw new LoginException("Username or password is incorrect.");
            }
            
        }
    }
}