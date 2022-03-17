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
                {Email = "realEmail@ForIntegrationTest.dummyTest", Password = "password", Username = "Radoslav"});
            var httpContext = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = client.PostAsync("/register", httpContext).Result;

            var responseString = response.Content.ReadAsStringAsync().Result;

            var responseObject = JsonConvert.DeserializeObject<RegisterUserResponseDTO>(responseString);
            
            //Assert
            Assert.Equal("Radoslav",responseObject.Username);
        }
    }
}