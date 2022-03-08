using System;
using System.Collections.Generic;
using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.Models;
using DotNetTribes.RegistrationExceptions;

namespace DotNetTribes.Services
{
    public class RegisterService : IRegisterService
    {
        private ApplicationContext _applicationContext;

        public RegisterService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        
        // TODO: check if kingdom is unique
        // TODO: check if email is unique
        public RegisterUserResponseDTO RegisterUser(RegisterUserRequestDTO userCredentials)
        {
            
            HandleMissingFields(userCredentials);
            
            if (UsernameIsTaken(userCredentials.Username))
            {
                throw new UsernameAlreadyTakenException();
            }
            
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

        // If one or more required fields are missing, it will throw exception.
        public void HandleMissingFields(RegisterUserRequestDTO userCredentials)
        {
            var errorMessages = new List<string>();
            
            if (userCredentials.Password == null)
            {
                errorMessages.Add("Password is required.");
            }

            if (userCredentials.Username == null)
            {
                errorMessages.Add("Username is required");
            }
            
            if (userCredentials.Email == null)
            {
                errorMessages.Add("Email is required");
            }
            
            if (errorMessages.Count > 0)
            {
                var errorOutput = String.Join(" ", errorMessages);
                throw new MissingFieldException(errorOutput);
            }
        }
        
        public bool UsernameIsTaken(string username)
        {
            var user = _applicationContext
                .Users
                .SingleOrDefault(user => user.Username == username);
            
            return (user != null);

        }
    }
}