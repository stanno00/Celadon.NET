namespace DotNetTribes.DTOs
{
    public class ForgotPasswordRequestDto
    {
        public string? AnswerSecretQuestion { get; set; }
        public string UserEmail { get; set; }
    }
}