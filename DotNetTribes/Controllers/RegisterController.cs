using System;
using DotNetTribes.DTOs;
using DotNetTribes.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTribes.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RegisterController
    {
        private IRegisterService _registerService;

        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
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
                return new CreatedResult("", _registerService.RegisterUser(userRequestCredentials));
            }

            catch (Exception exception)
            {
                return new BadRequestObjectResult( new { error = exception.Message});
            }
        }
    }
}