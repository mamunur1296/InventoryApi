using InventoryApi.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderMapingController : ControllerBase
    {
       
        [HttpPost("Create")]
        public async Task<IActionResult> Create(NewOrderDTOs model)
        {
            return Ok();
        }
    }
}
