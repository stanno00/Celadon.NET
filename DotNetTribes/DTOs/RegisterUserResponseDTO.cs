namespace DotNetTribes.DTOs
{
    public class RegisterUserResponseDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int KingdomId { get; set; }
        public long QuestionId { get; set; }
    }
}