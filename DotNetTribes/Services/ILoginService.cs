using System.Collections.Generic;
using System.Security.Claims;
using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public interface ILoginService
    {
        public LoginResponseDto VerifyUsernameAndPassword(LoginRequestDto usernamePasswordDto);
        
    }
}