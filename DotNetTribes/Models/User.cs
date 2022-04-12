namespace DotNetTribes.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public string Email { get; set; }
        public int KingdomId { get; set; }
        public Kingdom Kingdom { get; set; }
        public long SecurityQuestionId { get; set; }
        public SecurityQuestion SecurityQuestion { get; set; }
    }
}