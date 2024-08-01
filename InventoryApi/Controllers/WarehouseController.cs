﻿using InventoryApi.DTOs;
using InventoryApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IBaseServices<WarehouseDTOs> _service;

        public WarehouseController(IBaseServices<WarehouseDTOs> service)
        {
            _service = service;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(WarehouseDTOs model)
        {
            var result = await _service.CreateAsync(model);
            if (result.Success)
            {
                return StatusCode((int)HttpStatusCode.Created, new ResponseDTOs<string>
                {
                    Success = true,
                    Status = HttpStatusCode.Created,
                    Detail = "Warehouse Created  successfully !!."
                });
            }
            return StatusCode((int)HttpStatusCode.BadRequest, result);
        }
        [HttpGet("All")]
        public async Task<IActionResult> getAll()
        {
            var result = await _service.GetAllAsync();
            if (result.Success)
            {
                return StatusCode((int)HttpStatusCode.OK, new ResponseDTOs<IEnumerable<WarehouseDTOs>>
                {
                    Success = true,
                    Data = result.Data,
                    Status = HttpStatusCode.OK,
                    Detail = "Warehouse List   successfully !!."
                });
            }
            return StatusCode((int)HttpStatusCode.BadRequest, result);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _service.DeleteAsync(id);
            if (result.Success)
            {
                return StatusCode((int)HttpStatusCode.OK, new ResponseDTOs<string>
                {
                    Success = true,
                    Status = HttpStatusCode.OK,
                    Detail = "Warehouse deleted successfully"
                });
            }
            return StatusCode((int)HttpStatusCode.BadRequest, result);
        }
    }
}