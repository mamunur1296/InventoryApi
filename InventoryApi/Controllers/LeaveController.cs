using InventoryApi.DTOs;
using InventoryApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly IBaseServices<LeaveDTOs> _service;

        public LeaveController(IBaseServices<LeaveDTOs> service)
        {
            _service = service;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(LeaveDTOs model)
        {
            var result = await _service.CreateAsync(model);
            if (result)
            {
                return StatusCode((int)HttpStatusCode.Created, new ResponseDTOs<string>
                {
                    Success = true,
                    Status = HttpStatusCode.Created,
                    Detail = "Leave Created  successfully !!."
                });
            }
            return StatusCode((int)HttpStatusCode.BadRequest, result);
        }
        [HttpGet("All")]
        public async Task<IActionResult> getAll()
        {
            var result = await _service.GetAllAsync();
            if (result != null)
            {
                return StatusCode((int)HttpStatusCode.OK, new ResponseDTOs<IEnumerable<LeaveDTOs>>
                {
                    Success = true,
                    Data = result,
                    Status = HttpStatusCode.OK,
                    Detail = "Leave List   successfully !!."
                });
            }
            return StatusCode((int)HttpStatusCode.BadRequest, result);
        }
        [HttpGet("get/{id}")]
        public async Task<IActionResult> getById(string id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result != null)
            {
                return StatusCode((int)HttpStatusCode.OK, new ResponseDTOs<LeaveDTOs>
                {
                    Success = true,
                    Data = result,
                    Status = HttpStatusCode.OK,
                    Detail = "Leave  get   successfully !!."
                });
            }
            return StatusCode((int)HttpStatusCode.BadRequest, result);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _service.DeleteAsync(id);
            if (result)
            {
                return StatusCode((int)HttpStatusCode.OK, new ResponseDTOs<string>
                {
                    Success = true,
                    Status = HttpStatusCode.OK,
                    Detail = "Leave deleted successfully"
                });
            }
            return StatusCode((int)HttpStatusCode.BadRequest, result);
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, LeaveDTOs model)
        {
            var result = await _service.UpdateAsync(id, model);
            if (result)
            {
                return StatusCode((int)HttpStatusCode.OK, new ResponseDTOs<string>
                {
                    Success = true,
                    Status = HttpStatusCode.OK,
                    Detail = "Leave updated successfully"
                });
            }
            return StatusCode((int)HttpStatusCode.BadRequest, result);
        }
    }
}
