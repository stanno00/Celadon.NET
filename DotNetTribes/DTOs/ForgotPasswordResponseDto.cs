namespace DotNetTribes.DTOs
{
    public class ForgotPasswordResponseDto
    {
        public string? GeneratedPassword { get; set; }
        public string? SecretQuestion { get; set; }
    }
}