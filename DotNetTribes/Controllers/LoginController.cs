using System;
using DotNetTribes.DTOs;
using DotNetTribes.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTribes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public ActionResult Login([FromBody] UsernamePassowrdDto usernamePassowrdDto)
        {
            try
            {
                var token = _loginService.VerifiUsernameAndPassword(usernamePassowrdDto);
                return new CreatedResult("", token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestResult();
            }
        }
    }
}