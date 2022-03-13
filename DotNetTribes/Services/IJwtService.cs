namespace DotNetTribes.Services
{
    public interface IJwtService
    {
        string CreateToken(string name, string kingdomId);
        string GetNameFromJwt(string jwt);

        string GetKingdomIdFromJwt(string jwt);
    }
}