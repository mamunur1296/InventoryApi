using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class UserController : Controller
    {
        private readonly IClientServices<User> _userServices;
        private readonly IClientServices<Register> _registerServices;

        public UserController(IClientServices<User> userServices, IClientServices<Register> registerServices)
        {
            _userServices = userServices;
            _registerServices = registerServices;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetallUser()
        {
            var users = await _userServices.GetAllClientsAsync("User/GetAll");
            return Json(new { data = users });
        }
    }
}
