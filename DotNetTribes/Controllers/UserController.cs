using DotNetTribes.DTOs;
using DotNetTribes.DTOs.Password;
using DotNetTribes.DTOs.Trade;
using DotNetTribes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTribes.Controllers
{
    [ApiController]
    public class UserController
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UserController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        
        [HttpPost("/register")]
        public ActionResult RegisterNewUser([FromBody] RegisterUserRequestDTO registerUserRequestDTO)
        {
            var newUser = _userService.RegisterUser(registerUserRequestDTO);
            return new CreatedResult("", newUser);
        }

        [HttpPut("/user/password/{username}")]
        public AcceptedResult ForgotPassword([FromRoute] string username,
            [FromBody] ForgotPasswordRequestDto userInformation)
        {
            var newPassword = _userService.ForgottenPassword(username, userInformation);

            return new AcceptedResult("", newPassword);
        }

        [HttpPut("user")]
        [Authorize]
        public ActionResult<ResponseDTO> UpdatePassword([FromHeader] string authorization,
            [FromBody] PasswordRequestDto passwordRequestDto)
        {
            var username = _jwtService.GetNameFromJwt(authorization);
             _userService.ChangePassword(username, passwordRequestDto);
             return new OkObjectResult(new ResponseDTO()
             {
                 Status = "Password changed"
             });

        }
    }
}