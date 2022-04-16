using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using DotNetTribes;
using DotNetTribes.DTOs;
using DotNetTribes.DTOs.Troops;
using DotNetTribes.Enums;
using DotNetTribes.Exceptions;
using DotNetTribes.Models;
using DotNetTribes.Services;
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

            var token = jwtServiceTest.CreateToken("Hrnik", "1");

            //Act
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = client.GetAsync("/kingdom").Result;

            var responseString = response.Content.ReadAsStringAsync().Result;

            var responseObject = JsonConvert.DeserializeObject<KingdomDto>(responseString);

            //Assert
            Assert.Equal("Hrnik", responseObject.Username);
            Assert.Equal("Cool Kingdom Name", responseObject.KingdomName);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void KingdomController_NearestKingdoms_ReturnNearestKingdomsBasedOnMinutes()
        {
            // Arrange
            using var client = new CustomWebApplicationFactory<Startup>().CreateClient();
            var jwtServiceTest = new JwtService();

            var token = jwtServiceTest.CreateToken("Hrnik", "1");

            // Act
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = client.GetAsync("/kingdom/nearest/400").Result;

            var responseString = response.Content.ReadAsStringAsync().Result;

            var responseObjectList = JsonConvert.DeserializeObject<List<NearbyKingdomsDto>>(responseString);
            var myObj = responseObjectList[0];

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Second Cool Kingdom Name", myObj.KingdomName);
            Assert.Equal(2, myObj.KingdomId);
            Assert.Equal(20, myObj.MinutesToArrive);
        }

        [Fact]
        public void KingdomController_NearestKingdoms_ReturnErrorMessage()
        {
            // Arrange
            using var client = new CustomWebApplicationFactory<Startup>().CreateClient();
            var jwtServiceTest = new JwtService();

            var token = jwtServiceTest.CreateToken("Hrnik", "1");

            // Act
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = client.GetAsync("/kingdom/nearest/1").Result;

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void KingdomController_BuildingAddUpgrade_ReturnBuildingUpgrade()
        {
            // Arrange
            using var client = new CustomWebApplicationFactory<Startup>().CreateClient();
            
            var jwtServiceTest = new JwtService();
            
            var token = jwtServiceTest.CreateToken("Hrnik", "1");
            
            var json = JsonConvert.SerializeObject(new BuildingsUpgradesRequestDto(){UpgradeName = AllBuildingUpgrades.Ranger});
            
            var httpContext = new StringContent(json, Encoding.UTF8, "application/json");
            // Act
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = client.PostAsync("/kingdom/buildings/upgrades", httpContext).Result;

            var responseString = response.Content.ReadAsStringAsync().Result;

            var responseObject = JsonConvert.DeserializeObject<BuildingsUpgradesResponseDto>(responseString);
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Ranger",responseObject.Name);
            Assert.Equal(1,responseObject.KingdomId);
        }
        
        [Fact]
        public void KingdomController_CreateSpecialTroop_ReturnListOfTroops()
        {
            // Arrange
            using var client = new CustomWebApplicationFactory<Startup>().CreateClient();
            
            var jwtServiceTest = new JwtService();
            
            var token = jwtServiceTest.CreateToken("Hrnik", "1");
            
            var json = JsonConvert.SerializeObject(new TroopRequestDTO()
            {
                Name = BlackSmithTroops.Scout,
                NumberOfTroops = 1
            });
            
            var httpContext = new StringContent(json, Encoding.UTF8, "application/json");
            // Act
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = client.PostAsync("/kingdom/troops/blacksmith", httpContext).Result;

            var responseString = response.Content.ReadAsStringAsync().Result;

            var responseObject = JsonConvert.DeserializeObject<TroopResponseDTO>(responseString);
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(responseObject.NewTroops);
            Assert.Contains("Scout",responseObject.NewTroops[0].Name);
        }
    }
}