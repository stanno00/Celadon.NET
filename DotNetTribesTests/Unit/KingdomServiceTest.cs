using System.Collections.Generic;
using DotNetTribes;
using DotNetTribes.Models;
using DotNetTribes.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DotNetTribesTests.Unit
{
    public class KingdomServiceTest
    {
        [Fact]
        public void KingdomService_KingdomInfo_ReturnKingdomDto()
        {
            ICollection<Building> buildingsTest = new List<Building>();
            ICollection<Resource> resourceTest = new List<Resource>();
            ICollection<Troop> troopsTest = new List<Troop>();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("name")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            Mock<IResourceService> iResourceServiceMock = new Mock<IResourceService>();

            context.Kingdoms.Add(new Kingdom()
            {
                KingdomId = 1,
                Name = "Benq",
                Buildings = buildingsTest,
                Resources = resourceTest,
                Troops = troopsTest
            });

            context.Users.Add(new User()
            {
                Email = "email@email.dummy",
                Username = "Hrnik",
                UserId = 1,
                KingdomId = 1,
                HashedPassword = "password"
            });
            context.SaveChanges();

            var result = new KingdomService(context,iResourceServiceMock.Object).KingdomInfo(1);

            Assert.Equal("Benq", result.KingdomName);
            Assert.Equal("Hrnik", result.Username);
        }
    }
}