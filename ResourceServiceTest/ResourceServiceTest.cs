using System.Collections.Generic;
using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.Services;
using Moq;
using Xunit;


namespace ResourceServiceTest;

public class ResourceServiceTest
{
    [Fact]
    public void Kingdom_without_any_resources_returns_empty_list()
    {
        Mock<IResourceService> iResourceServiceMock = new Mock<IResourceService>();

        ICollection<ResourceDto> resources = new List<ResourceDto>();

        iResourceServiceMock.Setup(s => s.GetKingdomResources(1)).Returns(new ResourcesDto()
        {
            Resources = resources
        });
        
        Assert.Empty(resources);
        Assert.NotNull(resources);
    }
    
    [Fact]
    public void Service_returns_correct_values()
    {
        Mock<IResourceService> iResourceServiceMock = new Mock<IResourceService>();

        
        ICollection<ResourceDto> resources = new List<ResourceDto>();
        var resource = new ResourceDto()
        {
            Amount = 80,
            Type = "Food",
            UpdatedAt = 545877
        };
        
        resources.Add(resource);

        iResourceServiceMock.Setup(s => s.GetKingdomResources(1)).Returns(new ResourcesDto()
        {
            Resources = resources
        });
        
        Assert.NotEmpty(resources);
        Assert.NotNull(resources);
        Assert.Equal(80, resources.ToArray()[0].Amount);
        Assert.Equal(1, resources.Count);
    }
}