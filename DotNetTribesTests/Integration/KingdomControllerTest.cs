using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using DotNetTribes;
using DotNetTribes.DTOs;
using DotNetTribes.Models;
using DotNetTribes.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace DotNetTribesTests.Integration
{
    public class KingdomControllerTest
    {
        [Fact]
        public void KingdomController_KingdomInfo_ReturnKingdomDto()
        {
            //Arrange
            using var client = new CustomWebApplicationFactory<Startup>().CreateClient();
            var jwtServiceTest = new JwtService();
            
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("InMemoryDbForTesting")
                .Options;
            
            var context = new ApplicationContext(optionsBuilder);
            
            context.Kingdoms.Add(new Kingdom()
            {
                KingdomId = 1,
                Name = "Super name for kingdom",
                Buildings = new List<Building>(),
                Resources = new List<Resource>(),
                Troops = new List<Troop>()
            });
            
            var jsonCreateNewUser = JsonConvert.SerializeObject(new RegisterUserRequestDTO()
                {Email = "realEmail@Test.dummy", Password = "password", Username = "Robitusin",KingdomName = "Super name for kingdom"});
            var httpContextCreateNew = new StringContent(jsonCreateNewUser, Encoding.UTF8, "application/json");
            
            var responseCreateUser = client.PostAsync("/Register", httpContextCreateNew).Result;
            
            var jsonLogUser = JsonConvert.SerializeObject(new LoginRequestDto()
                { Password = "password", Username = "Robitusin"});
            var httpContextLogUser = new StringContent(jsonLogUser, Encoding.UTF8, "application/json");
            
            var responseLogUser = client.PostAsync("/login", httpContextLogUser).Result;
            
            var token = jwtServiceTest.CreateToken("Robitusin", "1");
            
            //Act
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = client.GetAsync("/kingdom").Result;
            
            var responseString = response.Content.ReadAsStringAsync().Result;
            
            var responseObject = JsonConvert.DeserializeObject<KingdomDto>(responseString);
            
            //Assert
            Assert.Equal("Robitusin",responseObject.Username);
            Assert.Equal("Super name for kingdom",responseObject.KingdomName);
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        }
    }
}