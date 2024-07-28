using InventoryApi.DTOs;
using InventoryApi.Services.Implementation;
using InventoryApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create( RoleDTO model)
        {
            var response = new ResponseDTOs<string>();
            var result = await _roleService.CreateRoleAsync(model.RoleName);
            if (result)
            {
                response.Success = true;
                response.Data = $" Role  Created Successfully!";
                response.Status = HttpStatusCode.Created;
            }
            return StatusCode((int)HttpStatusCode.Created, response);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _roleService.GetRolesAsync();
            var result = users.Select(x => new RoleDTO()
            {
                Id = x.id, 
                RoleName=x.roleName,
               
            }).ToList();

            var response = new ResponseDTOs<IEnumerable<RoleDTO>>
            {
                Success = true,
                Data = result,
                Status = HttpStatusCode.OK,
                Detail = "All roles retrieved successfully."
            };

            return StatusCode((int)HttpStatusCode.OK, response);
        }
        [HttpGet("{RoleId}")]
        public async Task<IActionResult> GetDetails(string RoleId)
        {
            var role = await _roleService.GetRoleByIdAsync(RoleId);
            if (role.id == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound, new ResponseDTOs<RoleDTO>
                {
                    Success = false,
                    Status = HttpStatusCode.NotFound,
                    Detail = "Role not found."
                });
            }
            var RoleDTO = new RoleDTO
            {
                RoleName = role.roleName,
                Id=role.id,
            };

            var response = new ResponseDTOs<RoleDTO>
            {
                Success = true,
                Data = RoleDTO,
                Status = HttpStatusCode.OK,
                Detail = "Role details retrieved successfully."
            };

            return StatusCode((int)HttpStatusCode.OK, response);
        }
        [HttpDelete("{roleId}")]
        public async Task<IActionResult> Delete(string roleId)
        {
            var result = await _roleService.DeleteRoleAsync(roleId);
            if (!result)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseDTOs<string>
                {
                    Success = false,
                    Status = HttpStatusCode.InternalServerError,
                    Detail = "An error occurred while deleting the Role."
                });
            }

            var response = new ResponseDTOs<string>
            {
                Success = true,
                Data = roleId,
                Status = HttpStatusCode.OK,
                Detail = "Role deleted successfully."
            };

            return StatusCode((int)HttpStatusCode.OK, response);
        }
        [HttpPut("Edit/{Roleid}")]
        public async Task<ActionResult> Edit(string Roleid, RoleDTO model)
        {
            // Attempt to update the user profile
            var updateRole = await _roleService.UpdateRole(Roleid,model.RoleName);
            if (updateRole)
            {
                return StatusCode((int)HttpStatusCode.Created, new ResponseDTOs<string>
                {
                    Success = true,
                    Status = HttpStatusCode.Created,
                    Detail = " Role updated successfully."
                });
            }
            else
            {
                return StatusCode((int)HttpStatusCode.NotFound, new ResponseDTOs<string>
                {
                    Success = false,
                    Status = HttpStatusCode.NotFound,
                    Detail = "Role not found or update failed."
                });
            }
        }
    }



}
