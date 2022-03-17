using System.Threading.Tasks;
using DotNetTribes.Controllers;
using DotNetTribes.DTOs;
using DotNetTribes.Services;
using Moq;
using Xunit;

namespace DotNetTribesTests.Unit
{
    public class UserControllerTest
    {
        [Fact]
        public async Task UserController_RegisterNewUser_newUser()
        {
            //Arrange
            Mock<IUserService> iUserServiceMock = new Mock<IUserService>();
            var controller = new UserController(iUserServiceMock.Object);

            var userTest = new RegisterUserResponseDTO()
            {
                Username = "Hrnik",
                KingdomId = 1,
            };

            var registerUserRequestDtoTest = new RegisterUserRequestDTO()
            {
                Email = "email@email.dummy",
                Password = "password",
                Username = "Hrnik",
                KingdomName = "the lost kingdom of Benwick"
            };

            //Act
            iUserServiceMock.Setup(u=>u.RegisterUser(registerUserRequestDtoTest)).Returns(userTest);

            // var actionResult = await controller.RegisterNewUser(registerUserRequestDtoTest).ExecuteResultAsync();

            //Assert

            // var result = actionResult.Result as CreatedResult;
            // var user = Assert.IsType<RegisterUserResponseDTO>(result.Value);
            // Assert.NotNull(result);
            // Assert.Equal("hrnik",user.Username);
        }
    }
}