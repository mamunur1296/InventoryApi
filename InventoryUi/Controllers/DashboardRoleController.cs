using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    [Authorize]
    public class DashboardRoleController : Controller
    {
        private readonly IClientServices<Roles> _roleServices;

        public DashboardRoleController(IClientServices<Roles> roleServices)
        {
            _roleServices = roleServices;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var roles = await _roleServices.GetAllClientsAsync("Role/GetAll");
            return Json(roles);
        }
    }
}
