namespace DotNetTribes.Services
{
    public interface IAuthService
    {
        public string GetNameFromJwt(string Jwt);

        public int GetKingdomIdFromJwt(string Jwt);
    }
} 