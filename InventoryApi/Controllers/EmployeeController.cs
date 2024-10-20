using InventoryApi.DTOs;
using InventoryApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class EmployeeController : ControllerBase
    {
        private readonly IBaseServices<EmployeeDTOs> _service;
        public readonly IHelperServicess _helper;

        public EmployeeController(IBaseServices<EmployeeDTOs> service, IHelperServicess helper)
        {
            _service = service;
            _helper = helper;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(EmployeeDTOs model)
        {
            var result = await _service.CreateAsync(model);
            if (result)
            {
                return StatusCode((int)HttpStatusCode.Created, new ResponseDTOs<string>
                {
                    Success = true,
                    Status = HttpStatusCode.Created,
                    Detail = "Employee Created  successfully !!."
                });
            }
            return StatusCode((int)HttpStatusCode.BadRequest, result);
        }
        [HttpGet("CreateByAdmin/{id}")]
        public async Task<IActionResult> CreateByAdmin(string id)
        {
            var result = await _helper.CreateEmployeeByAdmin(id);
            if (result.isSucceed)
            {
                return StatusCode((int)HttpStatusCode.Created, new ResponseDTOs<string>
                {
                    Success = true,
                    Status = HttpStatusCode.Created,
                    Detail = "Employee Created  successfully !!."
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
                return StatusCode((int)HttpStatusCode.OK, new ResponseDTOs<IEnumerable<EmployeeDTOs>>
                {
                    Success = true,
                    Data = result,
                    Status = HttpStatusCode.OK,
                    Detail = "Employee List   successfully !!."
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
                    Detail = "Employee deleted successfully"
                });
            }
            return StatusCode((int)HttpStatusCode.BadRequest, result);
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, EmployeeDTOs model)
        {
            var result = await _service.UpdateAsync(id, model);
            if (result)
            {
                return StatusCode((int)HttpStatusCode.OK, new ResponseDTOs<string>
                {
                    Success = true,
                    Status = HttpStatusCode.OK,
                    Detail = "Employee updated successfully"
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
                return StatusCode((int)HttpStatusCode.OK, new ResponseDTOs<EmployeeDTOs>
                {
                    Success = true,
                    Data = result,
                    Status = HttpStatusCode.OK,
                    Detail = "Employee  get   successfully !!."
                });
            }
            return StatusCode((int)HttpStatusCode.BadRequest, result);
        }
    }
}
