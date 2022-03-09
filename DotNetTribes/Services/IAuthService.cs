
namespace DotNetTribes.Services
{
    public interface IAuthService
    {
        string SecretKey { get; set; }
        
        string GenerateToken(IAuthContainerService model);

        public string GetNameFromJwt(string Jwt);

        public string GetKingdomIdFromJwt(string Jwt);
    }
}