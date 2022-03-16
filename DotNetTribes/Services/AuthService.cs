using System;
using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.Exceptions;

namespace DotNetTribes.Services
{
    public class AuthService : IAuthService
    {
        
        private readonly ApplicationContext _applicationContext;
        private readonly IJwtService _jwtService;

        public AuthService(ApplicationContext applicationContext, IJwtService jwtService)
        {
            _applicationContext = applicationContext;
            _jwtService = jwtService;
        }

        public LoginResponseDto Login(LoginRequestDto usernamePasswordDto)
        {
            if (usernamePasswordDto == null || string.IsNullOrEmpty(usernamePasswordDto.Username) &&
                                                string.IsNullOrEmpty(usernamePasswordDto.Password))
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

            string token = _jwtService.CreateToken(user.Username, user.KingdomId.ToString());
            
            
            return new LoginResponseDto()
            {
                Status = "ok",
                Token = token
            };
        }

        private void isUsernameAndPasswordCorrect(LoginRequestDto usernamePasswordDto)
        {
            var user = _applicationContext.Users?.SingleOrDefault(u => u.Username == usernamePasswordDto.Username);
            if (user == null)
            {
                throw new LoginException("Username or password is incorrect.");
            }
            bool verified = BCrypt.Net.BCrypt.Verify(usernamePasswordDto.Password, user.HashedPassword);
            if (!verified)
            {
                throw new LoginException("Username or password is incorrect.");
            }
        }
    }
}