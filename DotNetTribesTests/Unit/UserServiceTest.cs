using System.Diagnostics;
using DotNetTribes;
using DotNetTribes.DTOs;
using DotNetTribes.Enums;
using DotNetTribes.Models;
using DotNetTribes.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DotNetTribesTests.Unit
{
    public class UserServiceTest
    {
        [Fact]
        public void UserService_RegisterUser_ReturnRegisterUserResponseDTO()
        {
            //Arrange
            Mock<IRulesService> iRuleServiceMock = new Mock<IRulesService>();
            Mock<ITimeService> iTimeServiceMock = new Mock<ITimeService>();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("User")
                .Options;

            var context = new ApplicationContext(optionsBuilder);

            var registerUserRequestDtoTest = new RegisterUserRequestDTO()
            {
                Email = "email@email.dummy",
                Password = "password",
                Username = "Hrnik",
                KingdomName = "the lost kingdom of Benwick",
                SecurityQuestionType = SecurityQuestionType.NameOfYourFirstPet,
                AnswerToQuestion = "pet"
            };

            //Act
            var result = new UserService(context,iRuleServiceMock.Object, iTimeServiceMock.Object).RegisterUser(registerUserRequestDtoTest);

            //Assert
            Assert.IsType<RegisterUserResponseDTO>(result);
            Assert.Equal("Hrnik", result.Username);
            Assert.Equal(1, result.Id);
            Assert.Equal(1, result.KingdomId);
        }

        [Fact]
        public void UserService_RegisterUser_ReturnErrorMissingPassword()
        {
            //Arrange
            Mock<IRulesService> iRuleServiceMock = new Mock<IRulesService>();
            Mock<ITimeService> iTimeServiceMock = new Mock<ITimeService>();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("password")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object, iTimeServiceMock.Object);

            var registerUserRequestDtoTest = new RegisterUserRequestDTO()
            {
                Email = "email@email.dummy",
                Username = "Hrnik",
                KingdomName = "the lost kingdom of Benwick",
                SecurityQuestionType = SecurityQuestionType.NameOfYourFirstPet,
                AnswerToQuestion = "pet"
            };

            //Act
            var exception = Record.Exception(() => controller.RegisterUser(registerUserRequestDtoTest));

            //Assert
            Debug.Assert(exception != null, nameof(exception) + " != null");
            Assert.Equal("Password is required.", exception.Message);
        }

        [Fact]
        public void UserService_RegisterUser_ReturnErrorMissingEmail()
        {
            //Arrange
            Mock<IRulesService> iRuleServiceMock = new Mock<IRulesService>();
            Mock<ITimeService> iTimeServiceMock = new Mock<ITimeService>();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("email")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object, iTimeServiceMock.Object);

            var registerUserRequestDtoTest = new RegisterUserRequestDTO()
            {
                Password = "password",
                Username = "Hrnik",
                KingdomName = "the lost kingdom of Benwick",
                SecurityQuestionType = SecurityQuestionType.NameOfYourFirstPet,
                AnswerToQuestion = "pet"
            };

            //Act
            var exception = Record.Exception(() => controller.RegisterUser(registerUserRequestDtoTest));

            //Assert
            Debug.Assert(exception != null, nameof(exception) + " != null");
            Assert.Equal("Email is required.", exception.Message);
        }

        [Fact]
        public void UserService_RegisterUser_ReturnErrorMissingUsername()
        {
            //Arrange
            Mock<IRulesService> iRuleServiceMock = new Mock<IRulesService>();
            Mock<ITimeService> iTimeServiceMock = new Mock<ITimeService>();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("username")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object, iTimeServiceMock.Object);

            var registerUserRequestDtoTest = new RegisterUserRequestDTO()
            {
                Email = "email@email.dummy",
                Password = "password",
                KingdomName = "the lost kingdom of Benwick",
                SecurityQuestionType = SecurityQuestionType.NameOfYourFirstPet,
                AnswerToQuestion = "pet"
            };

            //Act
            var exception = Record.Exception(() => controller.RegisterUser(registerUserRequestDtoTest));

            //Assert
            Debug.Assert(exception != null, nameof(exception) + " != null");
            Assert.Equal("Username is required.", exception.Message);
        }

        [Fact]
        public void UserService_RegisterUser_ReturnErrorShortPassword()
        {
            //Arrange
            Mock<IRulesService> iRuleServiceMock = new Mock<IRulesService>();
            Mock<ITimeService> iTimeServiceMock = new Mock<ITimeService>();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("passwordShort")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object, iTimeServiceMock.Object);

            var registerUserRequestDtoTest = new RegisterUserRequestDTO()
            {
                Email = "email@email.dummy",
                Password = "short",
                Username = "Hrnik",
                KingdomName = "the lost kingdom of Benwick",
                SecurityQuestionType = SecurityQuestionType.NameOfYourFirstPet,
                AnswerToQuestion = "pet"
            };

            //Act
            var exception = Record.Exception(() => controller.RegisterUser(registerUserRequestDtoTest));

            //Assert
            Debug.Assert(exception != null, nameof(exception) + " != null");
            Assert.Equal("Password must be at least 8 characters", exception.Message);
        }

        [Fact]
        public void UserService_RegisterUser_ReturnErrorKingdomNameAlreadyExist()
        {
            //Arrange
            Mock<IRulesService> iRuleServiceMock = new Mock<IRulesService>();
            Mock<ITimeService> iTimeServiceMock = new Mock<ITimeService>();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("kingdom name")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object, iTimeServiceMock.Object);

            var registerUserRequestDtoTest = new RegisterUserRequestDTO()
            {
                Email = "email@email.dummy",
                Password = "short",
                Username = "Hrnik",
                KingdomName = "the lost kingdom of Benwick",
                SecurityQuestionType = SecurityQuestionType.NameOfYourFirstPet,
                AnswerToQuestion = "pet"
            };
            var kingdomTest = new Kingdom()
            {
                Name = "the lost kingdom of Benwick"
            };

            context.Kingdoms.Add(kingdomTest);
            context.SaveChanges();

            //Act
            var exception = Record.Exception(() => controller.RegisterUser(registerUserRequestDtoTest));

            //Assert
            Debug.Assert(exception != null, nameof(exception) + " != null");
            Assert.Equal("Kingdom name is already taken.", exception.Message);
        }

        [Fact]
        public void UserService_RegisterUser_ReturnErrorUserNameAlreadyExist()
        {
            //Arrange
            Mock<IRulesService> iRuleServiceMock = new Mock<IRulesService>();
            Mock<ITimeService> iTimeServiceMock = new Mock<ITimeService>();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("Username exist")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object, iTimeServiceMock.Object);

            var registerUserRequestDtoTest = new RegisterUserRequestDTO()
            {
                Username = "Hrnik",
                Email = "email@email.dummy",
                Password = "short",
                KingdomName = "the lost kingdom of Benwick",
                SecurityQuestionType = SecurityQuestionType.NameOfYourFirstPet,
                AnswerToQuestion = "pet"
            };
            var userTest = new User()
            {
                Username = "Hrnik"
            };
            context.Users.Add(userTest);
            context.SaveChanges();

            //Act
            var exception = Record.Exception(() => controller.RegisterUser(registerUserRequestDtoTest));

            //Assert
            Debug.Assert(exception != null, nameof(exception) + " != null");
            Assert.Equal("Username is already taken.", exception.Message);
        }

        [Fact]
        public void UserService_RegisterUser_ReturnErrorUserEmailAlreadyExist()
        {
            //Arrange
            Mock<IRulesService> iRuleServiceMock = new Mock<IRulesService>();
            Mock<ITimeService> iTimeServiceMock = new Mock<ITimeService>();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("email exist")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object, iTimeServiceMock.Object);

            var registerUserRequestDtoTest = new RegisterUserRequestDTO()
            {
                Username = "Hrnik",
                Email = "email@email.dummy",
                Password = "short",
                KingdomName = "the lost kingdom of Benwick",
                SecurityQuestionType = SecurityQuestionType.NameOfYourFirstPet,
                AnswerToQuestion = "pet"
            };
            var userTest = new User()
            {
                Email = "email@email.dummy"
            };
            context.Users.Add(userTest);
            context.SaveChanges();

            //Act
            var exception = Record.Exception(() => controller.RegisterUser(registerUserRequestDtoTest));

            //Assert
            Debug.Assert(exception != null, nameof(exception) + " != null");
            Assert.Equal("Email address is already taken.", exception.Message);
        }
        
        [Fact]
        public void UserService_ForgottenPassword_ReturnErrorUserDoesNotExist()
        {
            //Arrange
            Mock<IRulesService> iRuleServiceMock = new Mock<IRulesService>();
            Mock<ITimeService> iTimeServiceMock = new Mock<ITimeService>();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("ReturnErrorUserDoesNotExist")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object, iTimeServiceMock.Object);

            var user = "non Existing user";
            var userInformation = new ForgotPasswordRequestDto()
            {
                UserEmail = "fake Email",
                AnswerSecretQuestion = "answer"
            };
            
            //Act
            var exception = Record.Exception(() => controller.ForgottenPassword(user,userInformation));
            
            //Assert
            Assert.Equal("User with this name does not exist", exception.Message);
        }
        
        [Fact]
        public void UserService_ForgottenPassword_ReturnErrorIncorrectEmail()
        {
            //Arrange
            Mock<IRulesService> iRuleServiceMock = new Mock<IRulesService>();
            Mock<ITimeService> iTimeServiceMock = new Mock<ITimeService>();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("IncorrectEmail")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object, iTimeServiceMock.Object);

            context.Users.Add(new User()
            {
                Username = "Jerzy",
                Email = "test@email.com"
            });

            context.SaveChanges();
            
            var user = "Jerzy";
            var userInformation = new ForgotPasswordRequestDto()
            {
                UserEmail = "fake Email",
                AnswerSecretQuestion = "answer"
            };
            
            //Act
            var exception = Record.Exception(() => controller.ForgottenPassword(user,userInformation));
            
            //Assert
            Assert.Equal("Incorrect email", exception.Message);
        }
        
        [Fact]
        public void UserService_ForgottenPassword_ReturnQuestion()
        {
            //Arrange
            Mock<IRulesService> iRuleServiceMock = new Mock<IRulesService>();
            Mock<ITimeService> iTimeServiceMock = new Mock<ITimeService>();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("ReturnQuestion")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object, iTimeServiceMock.Object);

            context.Users.Add(new User()
            {
                Username = "Jerzy",
                Email = "test@email.com",
                SecurityQuestion = new SecurityQuestion()
                {
                    Answer = "Answer to the question",
                    TheQuestion = SecurityQuestionType.CityYouWhereBornIn,
                    SecurityQuestionId = 1
                }
            });

            context.SaveChanges();
            
            var user = "Jerzy";
            var userInformation = new ForgotPasswordRequestDto()
            {
                UserEmail = "test@email.com",
            };
            
            //Act
            var result = controller.ForgottenPassword(user,userInformation);
                
            //Assert
            Assert.Equal(SecurityQuestionType.CityYouWhereBornIn.ToString(), result.SecretQuestion.ToString());
        }
        
        [Fact]
        public void UserService_ForgottenPassword_ReturnErrorIncorrectAnswer()
        {
            //Arrange
            Mock<IRulesService> iRuleServiceMock = new Mock<IRulesService>();
            Mock<ITimeService> iTimeServiceMock = new Mock<ITimeService>();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("IncorrectAnswer")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object, iTimeServiceMock.Object);

            context.Users.Add(new User()
            {
                Username = "Jerzy",
                Email = "test@email.com",
                SecurityQuestion = new SecurityQuestion()
                {
                    Answer = "$2a$11$pUwFwVARcS52cQoqvpj6j.u8BBa71a03EkHhbiXEAYJWBgiGzsexi",
                    TheQuestion = SecurityQuestionType.CityYouWhereBornIn,
                    SecurityQuestionId = 1
                }
            });

            context.SaveChanges();
            
            var user = "Jerzy";
            var userInformation = new ForgotPasswordRequestDto()
            {
                UserEmail = "test@email.com",
                AnswerSecretQuestion = "wrong answer"
            };
            
            //Act
            var exception = Record.Exception(() => controller.ForgottenPassword(user,userInformation));
            
            //Assert
            Assert.Equal("Answer to your secret question is not correct", exception.Message);
        }
        
        [Fact]
        public void UserService_ForgottenPassword_ReturnRandomPassword()
        {
            //Arrange
            Mock<IRulesService> iRuleServiceMock = new Mock<IRulesService>();
            Mock<ITimeService> iTimeServiceMock = new Mock<ITimeService>();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("ReturnRandomPassword")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object, iTimeServiceMock.Object);

            context.Users.Add(new User()
            {
                Username = "Jerzy",
                Email = "test@email.com",
                SecurityQuestion = new SecurityQuestion()
                {
                    Answer = "$2a$11$pUwFwVARcS52cQoqvpj6j.u8BBa71a03EkHhbiXEAYJWBgiGzsexi",
                    TheQuestion = SecurityQuestionType.CityYouWhereBornIn,
                    SecurityQuestionId = 1
                }
            });

            context.SaveChanges();
            
            var user = "Jerzy";
            var userInformation = new ForgotPasswordRequestDto()
            {
                UserEmail = "test@email.com",
                AnswerSecretQuestion = "R"
            };
            
            //Act
            var result = controller.ForgottenPassword(user,userInformation);
                
            //Assert
            Assert.IsType<ForgotPasswordResponseDto>(result);
        }
    }
}