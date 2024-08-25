using InventoryApi.DTOs;
using InventoryApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            var response =  new ResponseDTOs<IEnumerable<UserDTO>>
            {
                Success = true,
                Data = users,
                Status = HttpStatusCode.OK,
                Detail = "All users retrieved successfully."
            };

            return StatusCode((int)HttpStatusCode.OK, response);
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetDetails(string userId)
        {
            var user = await _userService.GetUserDetailsAsync(userId);
            if (user.Id == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound, new ResponseDTOs<UserDTO>
                {
                    Success = false,
                    Status = HttpStatusCode.NotFound,
                    Detail = "User not found."
                });
            }
            var response = new ResponseDTOs<UserDTO>
            {
                Success = true,
                Data = user,
                Status = HttpStatusCode.OK,
                Detail = "User details retrieved successfully."
            };

            return StatusCode((int)HttpStatusCode.OK, response);
        }
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            if (!result)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseDTOs<string>
                {
                    Success = false,
                    Status = HttpStatusCode.InternalServerError,
                    Detail = "An error occurred while deleting the user."
                });
            }

            var response = new ResponseDTOs<string>
            {
                Success = true,
                Data = userId,
                Status = HttpStatusCode.OK,
                Detail = "User deleted successfully."
            };

            return StatusCode((int)HttpStatusCode.OK,response);
        }
        [HttpPut("Edit/{id}")]
        public async Task<ActionResult> Edit(string id, UserDTO model)
        {
            // Attempt to update the user profile
            var updateUser = await _userService.UpdateUserProfile(id, model.FirstName, model.LastName, model.Email,model.UserImg,model.PhoneNumber,model.NID,model.Address,model.Job,model.Country,model.About, model.Roles);
            if (updateUser)
            {
                return StatusCode((int)HttpStatusCode.Created, new ResponseDTOs<string>
                {
                    Success = true,
                    Status = HttpStatusCode.Created,
                    Detail = "User profile updated successfully."
                });
            }
            else
            {
                return StatusCode((int)HttpStatusCode.NotFound, new ResponseDTOs<string>
                {
                    Success = false,
                    Status = HttpStatusCode.NotFound,
                    Detail = "User not found or update failed."
                });
            }
        }
        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword(ChangePassword model)
        {
            var (success, errorMessage) = await _userService.ChangePassword(model.OldPassword, model.NewPassword, model.UserId);

            if (success)
            {
                return Ok(new ResponseDTOs<string>
                {
                    Success = true,
                    Status = HttpStatusCode.OK,
                    Detail = "Password changed successfully."
                });
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new ResponseDTOs<string>
                {
                    Success = false,
                    Status = HttpStatusCode.BadRequest,
                    Detail = errorMessage
                });
            }
        }
    }
}
