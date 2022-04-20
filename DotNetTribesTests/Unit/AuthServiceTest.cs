using DotNetTribes;
using DotNetTribes.DTOs;
using DotNetTribes.Models;
using DotNetTribes.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DotNetTribesTests.Unit;

public class AuthServiceTest
{
    [Fact]
    public void AuthService_EmptyLoginRequestDto_ThrowsException()
    {
        var request = new LoginRequestDto()
        {
            Username = "",
            Password = ""
        };
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("AuthTest1")
            .Options;
        var context = new ApplicationContext(optionsBuilder);
        
        Mock<IJwtService> jwtService = new Mock<IJwtService>();
        var controller = new AuthService(context, jwtService.Object);
        var exception = Record.Exception(() => controller.Login(request));
        
        Assert.Equal("All fields are required.", exception.Message);

    }
    
    [Fact]
    public void AuthService_PasswordMissing_ThrowsException()
    {
        var request = new LoginRequestDto()
        {
            Username = "ja",
            Password = ""
        };
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("AuthTest1")
            .Options;
        var context = new ApplicationContext(optionsBuilder);
        
        Mock<IJwtService> jwtService = new Mock<IJwtService>();
        var controller = new AuthService(context, jwtService.Object);
        var exception = Record.Exception(() => controller.Login(request));
        
        Assert.Equal("Password is required.", exception.Message);

    }
    
    [Fact]
    public void AuthService_UsernameMissing_ThrowsException()
    {
        var request = new LoginRequestDto()
        {
            Username = "",
            Password = "password"
        };
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("AuthTest1")
            .Options;
        var context = new ApplicationContext(optionsBuilder);
        
        Mock<IJwtService> jwtService = new Mock<IJwtService>();
        var controller = new AuthService(context, jwtService.Object);
        var exception = Record.Exception(() => controller.Login(request));
        
        Assert.Equal("Username is required.", exception.Message);

    }
    
    [Fact]
    public void AuthService_WrongUsername_ThrowsException()
    {
        var request = new LoginRequestDto()
        {
            Username = "ja",
            Password = "password"
        };
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("AuthTest2")
            .Options;
        var context = new ApplicationContext(optionsBuilder);
        
        Mock<IJwtService> jwtService = new Mock<IJwtService>();
        var controller = new AuthService(context, jwtService.Object);
        var exception = Record.Exception(() => controller.Login(request));
        
        Assert.Equal("Username or password is incorrect.", exception.Message);

    }
    
    [Fact]
    public void AuthService_WrongPassword_ThrowsException()
    {
        var request = new LoginRequestDto()
        {
            Username = "Hrnik",
            Password = "passwor"
        };
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("AuthTest3")
            .Options;
        var context = new ApplicationContext(optionsBuilder);

        var user = new User()
        {
            Username = "Hrnik",
            HashedPassword = "$2a$11$EtdJ7HIRZihSF/WLmYf8HOnGD1VThPLGV3lg4PGnVan4IvOXD0.Ru"
        };
        context.Add(user);
        context.SaveChanges();
        
        Mock<IJwtService> jwtService = new Mock<IJwtService>();
        var controller = new AuthService(context, jwtService.Object);
        var exception = Record.Exception(() => controller.Login(request));
        
        Assert.Equal("Username or password is incorrect.", exception.Message);

    }
}