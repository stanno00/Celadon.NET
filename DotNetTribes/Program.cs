using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNetTribes.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotNetTribes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            
            IAuthContainerService model = getJwtContainerService("Moshe Binieli", "mmoshikoo@gmail.com");
            IAuthService authService = new JWTService(model.SecretKey);

            string token = authService.GenerateToken(model);

            if (!authService.IsTokenValid(token))
                throw new UnauthorizedAccessException();
            else
            {
                List<Claim> claims = authService.GetTokenClaims(token).ToList();

                Console.WriteLine(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);
                Console.WriteLine(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Email)).Value);
                Console.WriteLine(token);
            }
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        
        private static JWTContainerService getJwtContainerService(string name, string email)
        {
            return new JWTContainerService()
            {
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim("kindomID", email),
                }
            };
        }
    }
}