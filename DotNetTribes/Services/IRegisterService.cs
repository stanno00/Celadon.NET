using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public interface IRegisterService
    {
        RegisterUserResponseDTO RegisterUser(RegisterUserRequestDTO userCredentials);
        public void HandleMissingFields(RegisterUserRequestDTO userCredentials);
        public bool UsernameIsTaken(string username);
        public bool PasswordIsShort(int minLength, string password);
        public bool KingdomNameIsTaken(string kingdomName);
    }
}