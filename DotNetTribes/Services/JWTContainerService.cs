using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace DotNetTribes.Services
{
    public class JWTContainerService : IAuthContainerService
    {
        public string SecretKey { get; set; } = "TW9zaGVFcmV6UHJpdmF0ZUtleQ==";
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
        public int ExpireMinutes { get; set; } = 1440;  
        public Claim[] Claims { get; set; } 
    }
}