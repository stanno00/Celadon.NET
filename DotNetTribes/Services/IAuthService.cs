using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public interface IAuthService
    {
        LoginResponseDto Login(LoginRequestDto usernamePasswordDto);
    }
}
