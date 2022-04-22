using DotNetTribes.Services;
using Xunit;

namespace DotNetTribesTests.Unit;

public class JwtServiceTest
{
    [Fact]
    public void JwtService_GetNameFromJwt_ReturnUsername()
    {
        var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VybmFtZSI6ImphIiwiS2luZ2RvbUlkIjoiMiIsIm5iZiI6MTY0OTc5NjkxNCwiZXhwIjoxNjQ5ODA3NzE0LCJpYXQiOjE2NDk3OTY5MTR9.gIv1sYFHW-ZQY5iotgQVzgfu_lbTEHg4OPmSJLEUEIY";
        var jwtService = new JwtService();
        var noBearerToken = "Bearer " + token;
            
        Assert.Equal("ja", jwtService.GetNameFromJwt(token));
        Assert.Equal("ja", jwtService.GetNameFromJwt(noBearerToken));
    }
    
    [Fact]
    public void JwtService_GetKingdomIdFromJwt_ReturnKingdomId()
    {
        var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VybmFtZSI6ImphIiwiS2luZ2RvbUlkIjoiMiIsIm5iZiI6MTY0OTc5NjkxNCwiZXhwIjoxNjQ5ODA3NzE0LCJpYXQiOjE2NDk3OTY5MTR9.gIv1sYFHW-ZQY5iotgQVzgfu_lbTEHg4OPmSJLEUEIY";
        var jwtService = new JwtService();
        var noBearerToken = "Bearer " + token;

        var kingdomId = jwtService.GetKingdomIdFromJwt(token);
        var kingdomIdBearer = jwtService.GetKingdomIdFromJwt(noBearerToken);
            
        Assert.Equal(2, kingdomId);
        Assert.Equal(2, kingdomIdBearer);
    }
}