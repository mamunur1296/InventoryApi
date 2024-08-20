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
        [HttpPost]
        public async Task<IActionResult> Create(Roles model)
        {
            model.UpdatedBy = null;
            var register = await _roleServices.PostClientAsync("Role/Create", model);
            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var roles = await _roleServices.GetAllClientsAsync("Role/GetAll");
            return Json(roles);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _roleServices.DeleteClientAsync($"Role/{id}");
            return Json(deleted);
        }
        [HttpGet]
        public async Task<IActionResult> CheckDuplicate(string key, string val)
        {
            var usersResponse = await _roleServices.GetAllClientsAsync("Role/GetAll");
            if (usersResponse.Success)
            {
                bool isDuplicate = usersResponse.Data.Any(user =>
                {
                    var propertyInfo = user.GetType().GetProperty(key);
                    if (propertyInfo == null) return false;
                    var propertyValue = propertyInfo.GetValue(user, null)?.ToString();
                    return propertyValue?.Trim().Equals(val.Trim(), StringComparison.OrdinalIgnoreCase) ?? false;
                });

                return Json(isDuplicate);
            }
            return Json(false);
        }
    }
}
