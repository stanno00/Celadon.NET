using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public interface IRegisterService
    {
        RegisterUserResponseDTO RegisterUser(RegisterUserRequestDTO userCredentials);
        
    }
}