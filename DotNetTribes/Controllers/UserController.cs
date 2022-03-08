using System;
using DotNetTribes.DTOs;
using DotNetTribes.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTribes.Controllers
{
    [Route("/register")]
    [ApiController]
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /*
         * If post request has invalid field/fields, it will return JSON object with corresponding
         * error message (see RegistrationExceptions folder).
         */
        [HttpPost]
        public ActionResult RegisterNewUser([FromBody] RegisterUserRequestDTO userRequestCredentials)
        {
            try
            {
                return new CreatedResult("", _userService.RegisterUser(userRequestCredentials));
            }

            catch (Exception exception)
            {
                return new BadRequestObjectResult( new { error = exception.Message});
            }
        }
    }
}