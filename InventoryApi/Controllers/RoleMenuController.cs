using InventoryApi.DTOs;
using InventoryApi.Services.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleMenuController : ControllerBase
    {
        private readonly RoleMenuMappingService _roleMenuMappingService;

        public RoleMenuController(RoleMenuMappingService roleMenuMappingService)
        {
            _roleMenuMappingService = roleMenuMappingService;
        }

        [HttpPost("UpdateRoleMenuMapping")]
        public async Task<IActionResult> UpdateRoleMenuMapping( RoleMenuMappingDto dto)
        
        {
            if (dto == null || string.IsNullOrEmpty(dto.RoleId) || dto.MenuIds == null || dto.SubMenuIds == null)
            {
                return BadRequest("Invalid input data.");
            }

            try
            {
                Console.WriteLine("Received request to update role menu mappings");
                await _roleMenuMappingService.UpdateRoleMenuMappings(dto);
                return Ok("Role menu mapping updated successfully.");
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Controller error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetRoleMenuMapping/{roleId}")]
        public async Task<IActionResult> GetRoleMenuMapping(string roleId)
        {
            var mapping = await _roleMenuMappingService.GetRoleMenuMappings(roleId);
            return Ok(mapping);
        }
    }
}
