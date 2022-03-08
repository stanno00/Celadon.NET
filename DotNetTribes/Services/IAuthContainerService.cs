using System.Security.Claims;

namespace DotNetTribes.Services
{
    public interface IAuthContainerService
    {
        public string SecretKey { get; set; }
        public string SecurityAlgorithm { get; set; }
        public int ExpireMinutes { get; set; }
        
        Claim[] Claims { get; set; }
    }
}