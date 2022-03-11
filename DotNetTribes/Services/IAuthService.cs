namespace DotNetTribes.Services
{
    public interface IAuthService
    {
        public string GetNameFromJwt(string Jwt);

        public string GetKingdomIdFromJwt(string Jwt);
    }
} 