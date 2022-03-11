using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public interface IUserService
    {
        RegisterUserResponseDTO RegisterUser(RegisterUserRequestDTO userCredentials);
        string HashPassword(string password);
        void HandleMissingFields(RegisterUserRequestDTO userCredentials);
        void CheckIfFieldsAreNotTaken(RegisterUserRequestDTO userCredentials);
        void CheckIfPasswordIsLongEnough(string password, int minLength);
        bool UsernameIsTaken(string username);
        bool KingdomNameIsTaken(string name);
        bool EmailIsTaken(string email);
        string SetKingdomNameIfMissing(string kingdomName, string userName);
    }
}