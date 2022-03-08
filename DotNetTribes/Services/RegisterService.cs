using System.ComponentModel.DataAnnotations;
using DotNetTribes.DTOs;
using DotNetTribes.Models;

namespace DotNetTribes.Services
{
    public class RegisterService : IRegisterService
    {
        private ApplicationContext _applicationContext;

        public RegisterService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public RegisterUserResponseDTO RegisterUser(RegisterUserRequestDTO userCredentials)
        {
            string HashedPassword = BCrypt.Net.BCrypt.HashPassword(userCredentials.Password);

            var user = new User()
            {
                Username = userCredentials.Username,
                HashedPassword = HashedPassword,
                Email = userCredentials.Email,
                Kingdom = new Kingdom() { Name = userCredentials.Kingdomname}
            };

            _applicationContext.Add(user);
            _applicationContext.SaveChanges();
            
            return new RegisterUserResponseDTO()
            {
                Id = user.UserId,
                Username = user.Username,
                KingdomId = user.KingdomId
            };
        }
    }
}