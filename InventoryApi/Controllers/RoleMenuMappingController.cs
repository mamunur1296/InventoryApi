using InventoryApi.DTOs;
using InventoryApi.Services.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleMenuMappingController : ControllerBase
    {
        private readonly RoleMenuMappingService _service;

        public RoleMenuMappingController(RoleMenuMappingService service)
        {
            _service = service;
        }

        [HttpPost("UpdateRoleMenuMappings")]
        public async Task<IActionResult> UpdateRoleMenuMappings([FromBody] RoleMenuMappingDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.UpdateRoleMenuMappings(dto);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetRoleMenuMappings/{roleId}")]
        public async Task<IActionResult> GetRoleMenuMappings(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                return BadRequest("Invalid role ID.");
            }

            var result = await _service.GetRoleMenuMappings(roleId);
            return Ok(result);
        }
    }
}

