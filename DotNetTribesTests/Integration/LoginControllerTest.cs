using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DotNetTribes;
using DotNetTribes.DTOs;
using Newtonsoft.Json;
using Xunit;

namespace DotNetTribesTests.Integration;

public class LoginControllerTest
{
    [Fact]
    public async Task LoginController_Login_SuccessfulLogin()
    {
        //Arrange
        using var application = new CustomWebApplicationFactory<Startup>();
        using var client = application.CreateClient();

        var json = JsonConvert.SerializeObject(new LoginRequestDto()
            {Password = "password", Username = "Hrnik"});
        var httpContext = new StringContent(json, Encoding.UTF8, "application/json");

        //Act
        var response = client.PostAsync("/login", httpContext).Result;

        var responseString = response.Content.ReadAsStringAsync().Result;

        var responseObject = JsonConvert.DeserializeObject<LoginResponseDto>(responseString);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("ok", responseObject.Status);
    }
}