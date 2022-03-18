using System.Diagnostics;
using DotNetTribes;
using DotNetTribes.DTOs;
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
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("User")
                .Options;

            var context = new ApplicationContext(optionsBuilder);

            var registerUserRequestDtoTest = new RegisterUserRequestDTO()
            {
                Email = "email@email.dummy",
                Password = "password",
                Username = "Hrnik",
                KingdomName = "the lost kingdom of Benwick"
            };

            //Act
            var result = new UserService(context,iRuleServiceMock.Object).RegisterUser(registerUserRequestDtoTest);

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
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("password")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object);

            var registerUserRequestDtoTest = new RegisterUserRequestDTO()
            {
                Email = "email@email.dummy",
                Username = "Hrnik",
                KingdomName = "the lost kingdom of Benwick"
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
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("email")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object);

            var registerUserRequestDtoTest = new RegisterUserRequestDTO()
            {
                Password = "password",
                Username = "Hrnik",
                KingdomName = "the lost kingdom of Benwick"
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
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("username")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object);

            var registerUserRequestDtoTest = new RegisterUserRequestDTO()
            {
                Email = "email@email.dummy",
                Password = "password",
                KingdomName = "the lost kingdom of Benwick"
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
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("passwordShort")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object);

            var registerUserRequestDtoTest = new RegisterUserRequestDTO()
            {
                Email = "email@email.dummy",
                Password = "short",
                Username = "Hrnik",
                KingdomName = "the lost kingdom of Benwick"
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
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("kingdom name")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object);

            var registerUserRequestDtoTest = new RegisterUserRequestDTO()
            {
                Email = "email@email.dummy",
                Password = "short",
                Username = "Hrnik",
                KingdomName = "the lost kingdom of Benwick"
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
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("Username exist")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object);

            var registerUserRequestDtoTest = new RegisterUserRequestDTO()
            {
                Username = "Hrnik",
                Email = "email@email.dummy",
                Password = "short",
                KingdomName = "the lost kingdom of Benwick"
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
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("email exist")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new UserService(context,iRuleServiceMock.Object);

            var registerUserRequestDtoTest = new RegisterUserRequestDTO()
            {
                Username = "Hrnik",
                Email = "email@email.dummy",
                Password = "short",
                KingdomName = "the lost kingdom of Benwick"
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
    }
}