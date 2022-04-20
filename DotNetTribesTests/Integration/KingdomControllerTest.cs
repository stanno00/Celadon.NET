using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using DotNetTribes;
using DotNetTribes.DTOs;
using DotNetTribes.DTOs.Trade;
using DotNetTribes.DTOs.Troops;
using DotNetTribes.DTOs.University;
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

        [Fact]
        public void KingdomController_GetBuildings_ReturnListOfBuildings()
        {
            using var client = new CustomWebApplicationFactory<Startup>().CreateClient();
            var jwtServiceTest = new JwtService();
            var token = jwtServiceTest.CreateToken("Hrnik SecondTest", "2");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = client.GetAsync("/kingdom/buildings").Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            List<Building> emptyList = new List<Building>();
            
            Assert.Equal("{\"buildings\":[]}", responseString);
        }

        [Fact]
        public void KingdomController_PostOffer_CreateOffer()
        {
            using var client = new CustomWebApplicationFactory<Startup>().CreateClient();
            var jwtServiceTest = new JwtService();
            var timeServiceTest = new TimeService();
            var token = jwtServiceTest.CreateToken("Hrnik", "1");
            var offerJson = JsonConvert.SerializeObject(new TradeRequestDTO()
            {
                OfferedResource = new TypeAmountDTO()
                {
                    Type = ResourceType.Food,
                    Amount = 50
                },
                WantedResource = new TypeAmountDTO()
                {
                    Type = ResourceType.Gold,
                    Amount = 50
                }
            });
            var httpContext = new StringContent(offerJson, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = client.PostAsync("/kingdom/offer", httpContext).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<Offer>(responseString);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, responseObject.OfferId);
            Assert.Equal(50, responseObject.SellingAmount);
            Assert.Equal(ResourceType.Food, responseObject.SellingType);
            Assert.Equal(ResourceType.Gold, responseObject.PayingType);
            Assert.Equal(50, responseObject.PayingAmount);
            Assert.Equal(1, responseObject.SellerKingdomId);
            Assert.Equal(timeServiceTest.GetCurrentSeconds(), responseObject.CreatedAt);
            Assert.Equal(timeServiceTest.GetCurrentSeconds() + 60, responseObject.ExpireAt);
            Assert.False(responseObject.ResourceReturned);
            Assert.Null(responseObject.BuyerKingdomId);
        }

        [Fact]
        public void KingdomController_PutOffer_UpdateOffer()
        {
            using var client = new CustomWebApplicationFactory<Startup>().CreateClient();
            var jwtServiceTest = new JwtService();
            var timeServiceTest = new TimeService();
            var token = jwtServiceTest.CreateToken("Hrnik", "1");
            var httpContext = new StringContent("{}",Encoding.UTF8, "application/json");
        
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = client.PutAsync("/kingdom/offer/1",httpContext).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<AcceptOfferResponseDTO>(responseString);
         
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(timeServiceTest.GetCurrentSeconds(), responseObject.Offer.ExpireAt);
            Assert.Equal(1, responseObject.Offer.BuyerKingdomId);
            Assert.Equal(1, responseObject.Offer.OfferId);
            Assert.Equal(ResourceType.Gold, responseObject.Offer.PayingType);
            Assert.Equal(50, responseObject.Offer.PayingAmount);
            Assert.Equal(ResourceType.Food, responseObject.Offer.SellingType);
            Assert.Equal(60, responseObject.Offer.SellingAmount);
            Assert.Equal(2, responseObject.Offer.SellerKingdomId);
            Assert.False(responseObject.Offer.ResourceReturned);

        }

        [Fact]
        public void KingdomController_PostUniversity_ReturnUniversityUpgradeResponseDto()
        {
            using var client = new CustomWebApplicationFactory<Startup>().CreateClient();
            var jwtServiceTest = new JwtService();
            var timeServiceTest = new TimeService();
            var json = JsonConvert.SerializeObject(new UniversityUpgradeRequestDto()
            {
                UpgradeType = UpgradeType.BuildingBuildSpeed
            });
            var httpContext = new StringContent(json, Encoding.UTF8, "application/json");
            var token = jwtServiceTest.CreateToken("Hrnik", "1");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = client.PostAsync("/kingdom/university", httpContext).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<UniversityUpgradeResponseDto>(responseString);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(UpgradeType.BuildingBuildSpeed.ToString(), responseObject.UpgradeType);
            Assert.Equal(0, responseObject.CurrentLevel);
            Assert.Equal(timeServiceTest.GetCurrentSeconds(), responseObject.StartedAt);
            Assert.Equal(timeServiceTest.GetCurrentSeconds() + 10, responseObject.FinishedAt);
        }
    }
}