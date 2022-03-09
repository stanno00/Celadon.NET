using System;
using System.Linq;
using System.Security.Claims;
using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public class LoginService : ILoginService
    {
        private readonly ApplicationContext _applicationContext;

        public LoginService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public LoginDto VerifiUsernameAndPassword(UsernamePassowrdDto usernamePasswordDto)
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

            IAuthContainerService model = getJwtContainerService(user.Username, user.KingdomId.ToString());
            IAuthService authService = new JWTService(model.SecretKey);

            string token = authService.GenerateToken(model);
            
            
            return new LoginDto()
            {
                status = "ok",
                token = token
            };
        }

        private bool isUsernameAndPasswordCorrect(UsernamePassowrdDto usernamePasswordDto)
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
        //here are the information that the token stores
        private static JWTContainerService getJwtContainerService(string username, string kinfomId)
        {
            return new JWTContainerService()
            {
                Claims = new Claim[]
                {
                    new Claim("username", username),
                    new Claim("kindomID", kinfomId)
                }
            };
        }
    }
}