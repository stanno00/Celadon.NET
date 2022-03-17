using System.Collections.Generic;
using System.Diagnostics;
using DotNetTribes;
using DotNetTribes.Models;
using DotNetTribes.Services;
using Microsoft.EntityFrameworkCore;
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

            var result = new KingdomService(context).KingdomInfo(1);

            Assert.Equal("Benq", result.KingdomName);
            Assert.Equal("Hrnik", result.Username);
        }

        [Fact]
        public void KingdomService_KingdomInfo_ReturnErrorInvalidKingdomId()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("name")
                .Options;

            var context = new ApplicationContext(optionsBuilder);
            var controller = new KingdomService(context);

            var exception = Record.Exception(() => controller.KingdomInfo(0));

            Debug.Assert(exception != null, nameof(exception) + " != null");
            Assert.Equal("Kingdom with this Id does not exist", exception.Message);
        }
    }
}