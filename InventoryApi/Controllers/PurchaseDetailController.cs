using InventoryApi.DTOs;
using InventoryApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseDetailController : ControllerBase
    {
        private readonly IBaseServices<PurchaseDetailDTOs> _service;

        public PurchaseDetailController(IBaseServices<PurchaseDetailDTOs> service)
        {
            _service = service;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(PurchaseDetailDTOs model)
        {
            var result = await _service.CreateAsync(model);
            if (result)
            {
                return StatusCode((int)HttpStatusCode.Created, new ResponseDTOs<string>
                {
                    Success = true,
                    Status = HttpStatusCode.Created,
                    Detail = " Purchase Detail Created  successfully !!."
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
                return StatusCode((int)HttpStatusCode.OK, new ResponseDTOs<IEnumerable<PurchaseDetailDTOs>>
                {
                    Success = true,
                    Data = result,
                    Status = HttpStatusCode.OK,
                    Detail = "Purchase Detail List   successfully !!."
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
                return StatusCode((int)HttpStatusCode.OK, new ResponseDTOs<PurchaseDetailDTOs>
                {
                    Success = true,
                    Data = result,
                    Status = HttpStatusCode.OK,
                    Detail = "Purchase Detail  get   successfully !!."
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
                    Detail = "Purchase Detail deleted successfully"
                });
            }
            return StatusCode((int)HttpStatusCode.BadRequest, result);
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, PurchaseDetailDTOs model)
        {
            var result = await _service.UpdateAsync(id, model);
            if (result)
            {
                return StatusCode((int)HttpStatusCode.OK, new ResponseDTOs<string>
                {
                    Success = true,
                    Status = HttpStatusCode.OK,
                    Detail = "Purchase Detail updated successfully"
                });
            }
            return StatusCode((int)HttpStatusCode.BadRequest, result);
        }
    }
}
