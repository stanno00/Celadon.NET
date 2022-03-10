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
            
            // if all fields are not taken -> returns kingdom name that will be saved in database
            // throws corresponding error otherwise
            var kingdomName = CheckIfFieldsAreNotTaken(userCredentials);
            
            CheckIfPasswordIsLongEnough(userCredentials.Password, 8);
            
            var user = new User()
            {
                Username = userCredentials.Username,
                HashedPassword = HashPassword(userCredentials.Password),
                Email = userCredentials.Email,
                Kingdom = new Kingdom() { Name = kingdomName }
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
            
            if (FieldIsNullOrEmpty(userCredentials.Password))
            {
                errorMessages.Add("Password is required.");
            }

            if (FieldIsNullOrEmpty(userCredentials.Username))
            {
                errorMessages.Add("Username is required.");
            }
            
            if (FieldIsNullOrEmpty(userCredentials.Email))
            {
                errorMessages.Add("Email is required.");
            }
            
            if (errorMessages.Count > 0)
            {
                var errorOutput = String.Join(" ", errorMessages);
                throw new MissingFieldException(errorOutput);
            }
        }
        
        public bool FieldIsNullOrEmpty(string field)
        {
            return String.IsNullOrEmpty(field.Trim());
        }

        // throws an error if one of the fields is already taken
        public string CheckIfFieldsAreNotTaken(RegisterUserRequestDTO userCredentials)
        {
            var kingdomName = SetKingdomNameIfMissing(
                userCredentials.Kingdomname, userCredentials.Username
            );
            
            if (UsernameIsTaken(userCredentials.Username))
            {
                throw new UsernameAlreadyTakenException();
            }
            
            if (KingdomNameIsTaken(kingdomName))
            {
                throw new KingdomNameAlreadyTakenException();
            }
            
            if (EmailIsTaken(userCredentials.Email))
            {
                throw new EmailAlreadyTakenException();
            }

            return kingdomName;
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
            return (FieldIsNullOrEmpty(kingdomName))
                ? $"{userName}'s kingdom"
                : kingdomName;
        }
    }
}