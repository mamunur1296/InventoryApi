using InventoryApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderMapingController : ControllerBase
    {
       
        [HttpPost("Create")]
        public async Task<IActionResult> Create(NewOrderDTOs model)
        {
            return StatusCode((int)HttpStatusCode.Created, new ResponseDTOs<string>
            {
                Success = true,
                Status = HttpStatusCode.Created,
                Detail = "Order  successfully !!."
            });
        }
    }
}
