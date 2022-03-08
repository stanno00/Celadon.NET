using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace DotNetTribes.Services
{
    public interface IAuthService
    {
        string SecretKey { get; set; }

        bool IsTokenValid(string token);
        string GenerateToken(IAuthContainerService model);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}