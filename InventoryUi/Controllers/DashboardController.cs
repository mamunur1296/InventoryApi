using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IClientServices<User> _userServices;

        public DashboardController(IClientServices<User> userServices)
        {
            _userServices = userServices;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetNotApprovedEmployees()
        {
            var users = await _userServices.GetAllClientsAsync("User/GetAll");
            if (users?.Data != null)
            {
                var result = users.Data
                    .Where(item => item.isEmployee && !item.isApprovedByAdmin)
                    .ToList(); 
                return Json(result);
            }
            return Json(null);
        }


    }
}
