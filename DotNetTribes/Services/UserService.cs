using System;
using System.Collections.Generic;
using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.DTOs.Password;
using DotNetTribes.Enums;
using DotNetTribes.Exceptions;
using DotNetTribes.Models;
using DotNetTribes.RegistrationExceptions;

namespace DotNetTribes.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IRulesService _rules;
        private readonly ITimeService _timeService;

        public UserService(ApplicationContext applicationContext, IRulesService rules, ITimeService timeService)
        {
            _applicationContext = applicationContext;
            _rules = rules;
            _timeService = timeService;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private int[] KingdomCoordinates()
        {
            Random random = new Random();

            int coordinatesX = 0;
            int coordinatesY = 0;

            do
            {
                coordinatesX = random.Next(_rules.MapBoundariesX());
                coordinatesY = random.Next(_rules.MapBoundariesY());
            } while (_applicationContext.Kingdoms.Any(k => k.KingdomX == coordinatesX && k.KingdomY == coordinatesY));
            
            int[] coordinates = new int[2];
            coordinates[0] = coordinatesX;
            coordinates[1] = coordinatesY;
            return coordinates;
        }

        public RegisterUserResponseDTO RegisterUser(RegisterUserRequestDTO userCredentials)
        {
            HandleMissingFields(userCredentials);
            CheckIfFieldsAreNotTaken(userCredentials);
            CheckIfPasswordIsLongEnough(userCredentials.Password, 8);

            int[] coordinates = KingdomCoordinates();

            var user = new User()
            {
                Username = userCredentials.Username,
                HashedPassword = HashPassword(userCredentials.Password),
                Email = userCredentials.Email,
                SecurityQuestion = new SecurityQuestion()
                {
                    TheQuestion = userCredentials.SecurityQuestionType,
                    Answer = HashPassword(userCredentials.AnswerToQuestion),
                },
                Kingdom = new Kingdom()
                {
                    Name = userCredentials.KingdomName,
                    KingdomX = coordinates[0],
                    KingdomY = coordinates[1],
                    Resources = new List<Resource>
                    {
                        new Resource
                        {
                            Type = ResourceType.Gold,
                            Amount = _rules.StartingGold(),
                            UpdatedAt = _timeService.GetCurrentSeconds(),
                            Generation = 0
                        },
                        new Resource
                        {
                            Type = ResourceType.Food,
                            Amount = _rules.StartingFood(),
                            UpdatedAt = _timeService.GetCurrentSeconds(),
                            Generation = 0
                        }, new Resource
                        {
                            Type = ResourceType.Iron,
                            Amount = 0, 
                            UpdatedAt = _timeService.GetCurrentSeconds(),
                            Generation = 0
                        }
                    }
                }
            };

            _applicationContext.Add(user);
            _applicationContext.SaveChanges();

            return new RegisterUserResponseDTO()
            {
                Id = user.UserId,
                Username = user.Username,
                KingdomId = user.KingdomId,
                QuestionId = user.SecurityQuestionId
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
            
            if (userCredentials.SecurityQuestionType.ToString().Length == 0 )
            {
                errorMessages.Add("Security Question is required.");
            }

            if (FieldIsNullOrEmpty(userCredentials.AnswerToQuestion))
            {
                errorMessages.Add("An answer to security question is required.");
            }

            if (errorMessages.Count > 0)
            {
                var errorOutput = String.Join(" ", errorMessages);
                throw new MissingFieldException(errorOutput);
            }
        }

        public bool FieldIsNullOrEmpty(string field)
        {
            return (field is null || field.Trim().Length == 0);
        }

        // throws an error if one of the fields is already taken
        public void CheckIfFieldsAreNotTaken(RegisterUserRequestDTO userCredentials)
        {
            var kingdomName = SetKingdomNameIfMissing(
                userCredentials.KingdomName, userCredentials.Username
            );

            if (UsernameIsTaken(userCredentials.Username))
            {
                throw new UsernameAlreadyTakenException();
            }

            if (KingdomNameIsTaken(kingdomName))
            {
                throw new KingdomNameAlreadyTakenException();
            }

            userCredentials.KingdomName = kingdomName;


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
            return (FieldIsNullOrEmpty(kingdomName))
                ? $"{userName}'s kingdom"
                : kingdomName;
        }

        public void ChangePassword(string username, PasswordRequestDto passwordRequestDto)
        {
            if (string.IsNullOrEmpty(passwordRequestDto.OldPassword) ||
                string.IsNullOrEmpty(passwordRequestDto.NewPassword) ||
                string.IsNullOrEmpty(passwordRequestDto.ConfirmingNewPassword))
            {
                throw new PasswordChangeException("All fields are required");
            }

            var user = _applicationContext.Users.FirstOrDefault(u => u.Username == username);
            bool verified = BCrypt.Net.BCrypt.Verify(passwordRequestDto.OldPassword, user.HashedPassword);
            if (!verified)
            {
                throw new PasswordChangeException("Old password is incorrect");
            }


            if (BCrypt.Net.BCrypt.Verify(passwordRequestDto.NewPassword, user.HashedPassword))
            {
                throw new PasswordChangeException("New password can't be the same as old password");
            }

            CheckIfPasswordIsLongEnough(passwordRequestDto.NewPassword, 8);
            if (passwordRequestDto.NewPassword != passwordRequestDto.ConfirmingNewPassword)
            {
                throw new PasswordChangeException("New and confirmation password don't match");
            }

            user.HashedPassword = HashPassword(passwordRequestDto.NewPassword);
            _applicationContext.SaveChanges();
        }
    }
}