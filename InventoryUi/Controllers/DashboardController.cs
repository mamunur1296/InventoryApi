using InventoryUi.Models;
using InventoryUi.Services.Interface;
using InventoryUi.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IClientServices<User> _userServices;
        private readonly IClientServices<Company> _companyServices;
        
        public DashboardController(IClientServices<User> userServices, IClientServices<Company> companyServices)
        {
            _userServices = userServices;
            _companyServices = companyServices;
        }

        public async Task<IActionResult> Index()
        {
            return View(); // Still returning DashboirdVm
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
