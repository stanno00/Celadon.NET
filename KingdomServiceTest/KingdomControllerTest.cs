using System.Collections.Generic;
using DotNetTribes.Controllers;
using DotNetTribes.DTOs;
using DotNetTribes.Models;
using DotNetTribes.Service;
using DotNetTribes.Services;
using Moq;
using Xunit;

namespace KingdomServiceTest;

public class KingdomControllerTest
{
    [Fact]
    public void KingdomController_KingdomInfo_ReturnKingdomDto()
    {
        //Arrange
        Mock<IKingdomService> iKingdomServiceMock = new Mock<IKingdomService>();
        Mock<IAuthService> iAuthServiceMock = new Mock<IAuthService>();

        ICollection<Building> buildingsTest = new List<Building>();
        ICollection<Resource> resourceTest = new List<Resource>();
        ICollection<Troop> troopsTest = new List<Troop>();

        string token =
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VybmFtZSI6ImZpcnN0IiwiS2luZG9tSWQiOiIxIiwibmJmIjoxNjQ2OTg2MTY2LCJleHAiOjE2NDY5OTY5NjYsImlhdCI6MTY0Njk4NjE2Nn0.SWFZ1pL_No1ZCiGXTuHNFInqYcSDIDAw-ALy8GMIKlA";

        //Act
        iKingdomServiceMock.Setup(k => k.KingdomInfo(1)).Returns(new KingdomDto()
        {
            KingdomName = "Benq",
            Username = "Hrnik",
            Buildings = buildingsTest,
            Resources = resourceTest,
            Troops = troopsTest
        });
        iAuthServiceMock.Setup(k => k.GetKingdomIdFromJwt(token)).Returns(1);

        var kingdomDtoResult =
            new KingdomController(iKingdomServiceMock.Object, iAuthServiceMock.Object).KingdomInfo(token);

        //Assert
        var kingdom = Assert.IsType<KingdomDto>(kingdomDtoResult.Value);
        Assert.Equal("Benq", kingdom.KingdomName);
        Assert.Equal("Hrnik", kingdom.Username);
    }
}