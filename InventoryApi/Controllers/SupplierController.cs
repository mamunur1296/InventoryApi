﻿using InventoryApi.DTOs;
using InventoryApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IBaseServices<SupplierDTOs> _service;

        public SupplierController(IBaseServices<SupplierDTOs> service)
        {
            _service = service;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(SupplierDTOs model)
        {
            var result = await _service.CreateAsync(model);
            if (result)
            {
                return StatusCode((int)HttpStatusCode.Created, new ResponseDTOs<string>
                {
                    Success = true,
                    Status = HttpStatusCode.Created,
                    Detail = "Supplier Created  successfully !!."
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
                return StatusCode((int)HttpStatusCode.OK, new ResponseDTOs<IEnumerable<SupplierDTOs>>
                {
                    Success = true,
                    Data = result,
                    Status = HttpStatusCode.OK,
                    Detail = "Supplier List   successfully !!."
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
                    Detail = "Supplier deleted successfully"
                });
            }
            return StatusCode((int)HttpStatusCode.BadRequest, result);
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, SupplierDTOs model)
        {
            var result = await _service.UpdateAsync(id, model);
            if (result)
            {
                return StatusCode((int)HttpStatusCode.OK, new ResponseDTOs<string>
                {
                    Success = true,
                    Status = HttpStatusCode.OK,
                    Detail = "Supplier updated successfully"
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
                return StatusCode((int)HttpStatusCode.OK, new ResponseDTOs<SupplierDTOs>
                {
                    Success = true,
                    Data = result,
                    Status = HttpStatusCode.OK,
                    Detail = "Supplier  get   successfully !!."
                });
            }
            return StatusCode((int)HttpStatusCode.BadRequest, result);
        }
    }
}