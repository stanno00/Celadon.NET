using System;
using System.Collections.Generic;
using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.Models;
using DotNetTribes.RegistrationExceptions;

namespace DotNetTribes.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _applicationContext;

        public UserService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        
        public RegisterUserResponseDTO RegisterUser(RegisterUserRequestDTO userCredentials)
        {
            
            HandleMissingFields(userCredentials);
            CheckIfFieldsAreNotTaken(userCredentials);
            CheckIfPasswordIsLongEnough(userCredentials.Password, 8);
            
            var kingdomName = SetKingdomNameIfMissing(
                userCredentials.Kingdomname, userCredentials.Username
                );
            
            var user = new User()
            {
                Username = userCredentials.Username,
                HashedPassword = HashPassword(userCredentials.Password),
                Email = userCredentials.Email,
                Kingdom = new Kingdom() { Name = kingdomName}
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

        // If one or more required fields is missing, it will throw exception.
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

        // throws an error if one of the fields is already taken
        public void CheckIfFieldsAreNotTaken(RegisterUserRequestDTO userCredentials)
        {
            if (UsernameIsTaken(userCredentials.Username))
            {
                throw new UsernameAlreadyTakenException();
            }
            
            if (KingdomNameIsTaken(userCredentials.Kingdomname))
            {
                throw new KingdomNameAlreadyTakenException();
            }
            
            if (EmailIsTaken(userCredentials.Email))
            {
                throw new EmailAlreadyTakenException();
            }
        }

        public void CheckIfPasswordIsLongEnough(string password, int minLength)
        {
            if (password.Length < minLength)
            {
                throw new ShortPasswordException(minLength);
            }
        }
        
        public bool UsernameIsTaken(string username)
        {
            var user = _applicationContext
                .Users
                .SingleOrDefault(user => user.Username == username);
            
            return (user != null);

        }

        public bool KingdomNameIsTaken(string name)
        {
            var kingdomName = _applicationContext
                .Kingdoms
                .FirstOrDefault(kingdom => kingdom.Name == name);

            return (kingdomName != null);
        }

        public bool EmailIsTaken(string email)
        {
            var emailAddress = _applicationContext
                .Users
                .FirstOrDefault(user => user.Email == email);

            return (emailAddress != null);
        }

        public string SetKingdomNameIfMissing(string kingdomName, string userName)
        {
            return kingdomName ?? $"{userName}'s kingdom";
        }
    }
}