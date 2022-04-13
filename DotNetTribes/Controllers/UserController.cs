using DotNetTribes.DTOs;
using DotNetTribes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTribes.Controllers
{
    [ApiController]
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpPost("/register")]
        public ActionResult RegisterNewUser([FromBody] RegisterUserRequestDTO registerUserRequestDTO)
        {
            var newUser = _userService.RegisterUser(registerUserRequestDTO);
            return new CreatedResult("", newUser);
        }

        [HttpPut("/user/password/{username}")]
        public AcceptedResult ForgotPassword( [FromRoute] string username,[FromBody] ForgotPasswordRequestDto userInformation)
        {
            var newPassword = _userService.ForgottenPassword(username, userInformation);
            
            return new AcceptedResult("",newPassword);
        }
    }
}