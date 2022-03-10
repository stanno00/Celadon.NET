using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly ILoginService _loginService;
        private readonly IAuthService _authService;

        public LoginController(ILoginService loginService, IAuthService authService)
        {
            _loginService = loginService;
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login([FromBody] LoginRequestDto loginRequestDto)
        {
           
                var token = _loginService.VerifyUsernameAndPassword(loginRequestDto);

                return new CreatedResult("", token);
        }

        [HttpPost]
        [Route("/test")]
        // [Authorize]
        public string test([FromBody] LoginResponseDto token)
        {
            string name = _authService.GetNameFromJwt(token.token);
            Console.WriteLine(name);
            return name;
        }
    }
}