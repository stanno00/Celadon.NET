using Newtonsoft.Json;

namespace DotNetTribes.DTOs
{
    public class RegisterUserRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        
        // comment from Alex: "There is no need to set an empty string as a default value"
        public string Kingdomname { get; set; } = "";
        public string Email { get; set; }
        
    }
}