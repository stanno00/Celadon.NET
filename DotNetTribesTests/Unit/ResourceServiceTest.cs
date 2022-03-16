using System.Linq;
using DotNetTribes;
using DotNetTribes.Enums;
using DotNetTribes.Models;
using DotNetTribes.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DotNetTribesTests.Unit;

public class ResourceServiceTest
{
    [Fact]
    public void GetKingdomResources_returns_empty_list()
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("ResourceDatabase")
            .Options;

        using var context = new ApplicationContext(options);
        context.Resources.Add(new Resource()
        {
            Amount = 80,
            Generation = 5,
            Type = ResourceType.Food,
            KingdomId = 1,
            CreatedAt = 69857,
            ResourceId = 1
        });
            
        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();

        var resourceService = new ResourceService(context, timeServiceMock.Object);

        var resources = resourceService.GetKingdomResources(0);
        
        Assert.Empty(resources.Resources);
        Assert.NotNull(resources.Resources);

    }
    
    [Fact]
    public void GetKingdomResources_correct_values()
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("ResourceDatabase")
            .Options;

        using var context = new ApplicationContext(options);
        context.Resources.Add(new Resource()
        {
            Amount = 80,
            Generation = 5,
            Type = ResourceType.Food,
            KingdomId = 1,
            CreatedAt = 69857,
            ResourceId = 1
        });
            
        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();

        var resourceService = new ResourceService(context, timeServiceMock.Object);

        var resources = resourceService.GetKingdomResources(1);
            
        Assert.NotEmpty(resources.Resources);
        Assert.NotNull(resources);
        Assert.Equal(80, resources.Resources.ToArray()[0].Amount);
        Assert.Equal(1, resources.Resources.Count);
        Assert.Equal("Food", resources.Resources.ToArray()[0].Type);
    }
}