using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using DotNetTribes;
using DotNetTribes.Controllers;
using DotNetTribes.DTOs;
using DotNetTribes.Exceptions;
using DotNetTribes.Migrations;
using DotNetTribes.Models;
using DotNetTribes.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace KingdomServiceTest;

public class KingdomServiceTest
{
    [Fact]
    public void KingdomService_KingdomInfo_ReturnKingdom()
    {
        ICollection<Building> buildingsTest = new List<Building>();
        ICollection<Resource> resourceTest = new List<Resource>();
        ICollection<Troop> troopsTest = new List<Troop>();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("name")
            .Options;

        ApplicationContext ctx = new ApplicationContext(optionsBuilder);
        var controller = new KingdomService(ctx);

        using (ctx)
        {
            ctx.Kingdoms.Add(new Kingdom()
            {
                KingdomId = 1,
                Name = "Benq",
                Buildings = buildingsTest,
                Resources = resourceTest,
                Troops = troopsTest
            });
            ctx.Users.Add(new User()
            {
                Email = "email@email.dummy",
                Username = "Hrnik",
                UserId = 1,
                KingdomId = 1,
                HashedPassword = "password"
            });
            ctx.SaveChanges();
        }

        var result = new KingdomDto();
        using (ctx)
        {
            result = controller.KingdomInfo(1);
        }

        var kingdom = Assert.IsType<KingdomDto>(result);
        Assert.Equal("Benq", kingdom.KingdomName);
        Assert.Equal("Hrnik", kingdom.Username);
    }

    // Arrange
    //     var fakeKingdomDto = new KingdomDto()
    //     {
    //         KingdomName = "Benq",
    //         Username = "Hrnik",
    //         Buildings = buildingsTest,
    //         Resources = resourceTest,
    //         Troops = troopsTest
    //     };
    // var dataStore = A.Fake<IApplicationContext>();
    // var controller = new KingdomService(dataStore);
    //
    // var setMock = new Mock<DbSet<Kingdom>>();
    // Mock<IKingdomService> iKingdomServiceMock = new Mock<IKingdomService>();
    //
    // Mock<ApplicationContext> applicationContextMock = new Mock<ApplicationContext>(setMock);
    //     
    //     //Act
    //     
    //     iKingdomServiceMock.Setup(k => k.KingdomInfo(1))
    //         .Returns(new KingdomDto()
    //         {
    //             KingdomName = "Benq",
    //             Username = "Hrnik",
    //             Buildings = buildingsTest,
    //             Resources = resourceTest,
    //             Troops = troopsTest
    //         });
    //
    //     var kingdomDtoResult = new KingdomService(dataStore).KingdomInfo(1);
    //     
    //     //Assert
    //     var kingdom = Assert.IsType<KingdomDto>(kingdomDtoResult);
    //     Assert.Equal("Benq", kingdom.KingdomName);
    //     Assert.Equal("Hrnik", kingdom.Username);
    // }


    // var mockSet = new Mock<DbSet<Blog>>();
    //
    // var mockContext = new Mock<BloggingContext>();
    // mockContext.Setup(m => m.Blogs).Returns(mockSet.Object);
    //
    // var service = new BlogService(mockContext.Object);
    // service.AddBlog("ADO.NET Blog", "http://blogs.msdn.com/adonet");
    //
    // mockSet.Verify(m => m.Add(It.IsAny<Blog>()), Times.Once());
    // mockContext.Verify(m => m.SaveChanges(), Times.Once());
}