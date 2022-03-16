using DotNetTribes.DTOs;
using DotNetTribes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTribes.Controllers
{
    [ApiController]
    [Route("/login")]
    public class LoginController
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login([FromBody] LoginRequestDto loginRequestDto)
        {
           
                var loginResponseDto = _authService.Login(loginRequestDto);

                return new OkObjectResult(loginResponseDto);
        }
    }
}