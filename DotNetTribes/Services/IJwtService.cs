
namespace DotNetTribes.Services
{
    public interface IJwtService
    {
        public string CreateToken(string name, string kingdomId);
        
    }
}