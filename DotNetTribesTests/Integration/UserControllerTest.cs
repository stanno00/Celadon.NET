using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DotNetTribes;
using DotNetTribes.DTOs;
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
                {Email = "Test@Test.dummy", Password = "password", Username = "Rado"});
            var httpContext = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = client.PostAsync("/register", httpContext).Result;

            var responseString = response.Content.ReadAsStringAsync().Result;

            var responseObject = JsonConvert.DeserializeObject<RegisterUserResponseDTO>(responseString);

            //Assert
            Assert.Equal("Rado", responseObject.Username);
            Assert.Equal(2, responseObject.Id);
        }
    }
}