
using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public interface IAuthService
    {
        public LoginResponseDto Login(LoginRequestDto usernamePasswordDto);
        public string getNameFromJwt(string Jwt);

        public string getKingdomIdFromJwt(string Jwt);
    }
}