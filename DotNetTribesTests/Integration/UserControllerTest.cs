using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DotNetTribes;
using DotNetTribes.DTOs;
using DotNetTribes.DTOs.Password;
using DotNetTribes.Enums;
using DotNetTribes.Services;
using Newtonsoft.Json;
using Xunit;

namespace DotNetTribesTests.Integration
{
    public class UserControllerTest
    {
        [Fact]
        public async Task UserController_RegisterNewUser_newUser()
        {
            //Arrange
            using var application = new CustomWebApplicationFactory<Startup>();
            using var client = application.CreateClient();

            var json = JsonConvert.SerializeObject(new RegisterUserRequestDTO()
                {Email = "Test@Test.dummy", Password = "password", Username = "Rado",
                    SecurityQuestionType = SecurityQuestionType.NameOfYourFirstPet,
                    AnswerToQuestion = "pet"});
            var httpContext = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = client.PostAsync("/register", httpContext).Result;

            var responseString = response.Content.ReadAsStringAsync().Result;

            var responseObject = JsonConvert.DeserializeObject<RegisterUserResponseDTO>(responseString);

            //Assert
            Assert.Equal("Rado", responseObject.Username);
            Assert.Equal(3, responseObject.Id);
        }

        [Fact]
        public void UserController_UpdatePassword_ChangePassword()
        {
            using var client = new CustomWebApplicationFactory<Startup>().CreateClient();
            var jwtServiceTest = new JwtService();
            var token = jwtServiceTest.CreateToken("Hrnik SecondTest", "2");
            var json = JsonConvert.SerializeObject(new PasswordRequestDto()
            {
                OldPassword = "password",
                NewPassword = "password1",
                ConfirmingNewPassword = "password1"
            });
            var httpContext = new StringContent(json, Encoding.UTF8, "application/json");
            var loginJson = JsonConvert.SerializeObject(new LoginRequestDto()
            {
                Username = "Hrnik SecondTest",
                Password = "password1"
            });
            var httpContextLogin = new StringContent(loginJson, Encoding.UTF8, "application/json");
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = client.PutAsync("/user", httpContext).Result;
            var login = client.PostAsync("/login", httpContextLogin).Result;
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        }
    }
}