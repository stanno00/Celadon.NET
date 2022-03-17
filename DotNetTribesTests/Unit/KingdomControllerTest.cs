using System.Collections.Generic;
using DotNetTribes.Controllers;
using DotNetTribes.DTOs;
using DotNetTribes.Models;
using DotNetTribes.Services;
using Moq;
using Xunit;

namespace DotNetTribesTests.Unit
{
    
    public class KingdomControllerTest
    {

        [Fact]
        public void KingdomController_KingdomInfo_ReturnKingdomDto()
        {
            Mock<IJwtService> jwtServiceMock = new Mock<IJwtService>();
            Mock<IKingdomService> iKingdomServiceMock = new Mock<IKingdomService>();

            ICollection<Building> buildingsTest = new List<Building>();
            ICollection<Resource> resourceTest = new List<Resource>();
            ICollection<Troop> troopsTest = new List<Troop>();

            string token = "token format";

            jwtServiceMock.Setup(k => k.GetKingdomIdFromJwt(token)).Returns(1);

            iKingdomServiceMock.Setup(k => k.KingdomInfo(1)).Returns(new KingdomDto
            {
                KingdomName = "Benq",
                Username = "Hrnik",
                Buildings = buildingsTest,
                Resources = resourceTest,
                Troops = troopsTest
            });

            var result = new KingdomController(iKingdomServiceMock.Object, jwtServiceMock.Object).KingdomInfo(token);

            var kingdom = Assert.IsType<KingdomDto>(result.Value);
            Assert.Equal("Benq",kingdom.KingdomName);
            Assert.Equal("Hrnik",kingdom.Username);

        }
    }
}