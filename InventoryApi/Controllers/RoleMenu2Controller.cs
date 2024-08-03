using InventoryApi.DTOs;
using InventoryApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleMenu2Controller : ControllerBase
    {
        private readonly IRoleMenuService _service;

        public RoleMenu2Controller(IRoleMenuService service)
        {
            _service = service;
        }

        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleMenuMapping(Guid roleId)
        {
            var mapping = await _service.GetRoleMenuMappingAsync(roleId);
            return Ok(mapping);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRoleMenuMapping([FromBody] RoleMenuMapping roleMenuMapping)
        {
            await _service.UpdateRoleMenuMappingAsync(roleMenuMapping);
            return NoContent();
        }
        [HttpGet("mappingsByRoleId/{roleId}")]
        public async Task<IActionResult> GetMappingsByRoleId(Guid roleId)
        {
            var result = await _service.GetMappingsByRoleIdAsync(roleId);
            return Ok(result);
        }
    }
}
