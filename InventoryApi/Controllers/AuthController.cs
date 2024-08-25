using InventoryApi.DTOs;
using InventoryApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthController(IUserService userService, ITokenGenerator tokenGenerator)
        {
            _userService = userService;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register( [FromBody] RegistrationDTOs model)
        {
            var response = new ResponseDTOs<string>();

            if (model.Password != model.ConfirmationPassword)
            {
                return BadRequest("Password and ConfirmationPassword do not match.");
            }

            var result = await _userService.CreateUserAsync(model);
            if (result.isSucceed)
            {
                response.Success = true;
                response.Data = $"User id = {result.userId} Created Successfully!";
                response.Status = HttpStatusCode.Created;
                response.Detail = "User registered successfully.";
            }
            return StatusCode((int)HttpStatusCode.Created, response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LogInDTOs model)
        {
            var result = await _userService.SigninUserAsync(model.UserName, model.Password);
            if (!result)
            {
                throw new Exception("Invalid username or password");
            }
            var user = await _userService.GetUserDetailsAsync(await _userService.GetUserIdAsync(model.UserName));

            string token = _tokenGenerator.GenerateJWTToken((user.Id, user.UserName, user.FirstName, user.LastName, user.Email, user.UserImg, user.Roles));
            var auth =  new AuthDTO()
            {
                UserId = user.Id,
                Name = user.UserName,
                Token = token,
            };

            var response = new ResponseDTOs<AuthDTO>
            {
                Success = true,
                Data = auth,
                Status = HttpStatusCode.OK,
                Detail = "User Lgoin successfully."
            };

            return StatusCode((int)HttpStatusCode.OK, response);
        }
    }
}
