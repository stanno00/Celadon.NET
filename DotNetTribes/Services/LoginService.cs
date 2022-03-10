using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DotNetTribes.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace DotNetTribes.Services
{
    public class LoginService : ILoginService
    {
        private readonly ApplicationContext _applicationContext;
        //private readonly string SECRET_KEY;

        public LoginService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            //SECRET_KEY = Environment.GetEnvironmentVariable("SECRET_KEY");
        }

        public LoginResponseDto VerifyUsernameAndPassword(LoginRequestDto usernamePasswordDto)
        {
            if (usernamePasswordDto == null)
            {
                throw new ArgumentException("All fields are required.");
            }

            if (usernamePasswordDto.password == null)
            {
                throw new ArgumentException("Password is required.");
            }
            if (usernamePasswordDto.username == null)
            {
                throw new ArgumentException("Username is required.");
            }

            if (isUsernameAndPasswordCorrect(usernamePasswordDto) == false)
            {
                throw new ArgumentException("Username or password is incorrect.");
            }
            var user = _applicationContext.Users.SingleOrDefault(u => u.Username == usernamePasswordDto.username);

            string token = CreateToken(user.Username, user.KingdomId.ToString());
            
            
            return new LoginResponseDto()
            {
                status = "ok",
                token = token
            };
        }
        
        private string CreateToken(string name, string kingdomId)
        {
            var symmetricKey = Encoding.ASCII.GetBytes("VG9wIFNlY3JldA==");
            
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {//here are the information that the token stores and their names
                        new Claim("username", name),
                        new Claim("kindomId", kingdomId)
                    }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(securityTokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);

            return token;
        }

        private bool isUsernameAndPasswordCorrect(LoginRequestDto usernamePasswordDto)
        {
            var user = _applicationContext.Users.SingleOrDefault(u => u.Username == usernamePasswordDto.username);
            if (user == null)
            {
                return false;
            }
            bool verified = BCrypt.Net.BCrypt.Verify(usernamePasswordDto.password, user.HashedPassword);
            if (verified == false)
            {
                return false;
            }

            return true;
        }
    }
}