namespace DotNetTribes.DTOs
{
    public class RegisterUserRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Kingdomname { get; set; }
        public string Email { get; set; }
    }
}