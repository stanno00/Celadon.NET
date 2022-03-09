using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public interface ILoginService
    {
        public LoginDto VerifiUsernameAndPassword(UsernamePassowrdDto usernamePasswordDto);
    }
}