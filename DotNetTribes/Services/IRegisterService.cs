using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public interface IRegisterService
    {
        RegisterUserResponseDTO RegisterUser(RegisterUserRequestDTO userCredentials);
        string HashString(string input);
        void HandleMissingFields(RegisterUserRequestDTO userCredentials);
        void CheckIfFieldsAreNotTaken(RegisterUserRequestDTO userCredentials);
        void CheckIfPasswordIsLongEnough(string password, int minLength);
        bool UsernameIsTaken(string username);
        bool PasswordIsShort(int minLength, string password);
        bool KingdomNameIsTaken(string name);
        bool EmailIsTaken(string email);
        string SetKingdomNameIfMissing(string kingdomName, string userName);
    }
}