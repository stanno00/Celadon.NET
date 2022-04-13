namespace DotNetTribes.DTOs.Password
{
    public class PasswordRequestDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmingNewPassword { get; set; }
    }
}