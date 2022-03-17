using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DotNetTribes;
using DotNetTribes.Controllers;
using DotNetTribes.DTOs;
using DotNetTribes.Models;
using DotNetTribes.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace ResourceControllerTest;

public class ResourceController
{
    [Fact]
    public void ResourceController_GetKingdomResources_ReturnResourceByKingdomId()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name);

        using (ApplicationContext ctx = new ApplicationContext(optionsBuilder.Options))
        {
            var controller = new ResourceController();
        }


        List<ResourceDto> resourceDtos = new List<ResourceDto>();

        var resourceDto = new ResourceDto
        {
            Amount = 1000,
            Type = "Weed",
            UpdatedAt = 100
        };
        resourceDtos.Add(resourceDto);
    }
}