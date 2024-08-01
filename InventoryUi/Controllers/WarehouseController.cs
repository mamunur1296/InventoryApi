using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly IClientServices<Warehouse> _warehouseServices;

        public WarehouseController(IClientServices<Warehouse> warehouseServices)
        {
            _warehouseServices = warehouseServices;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Warehouse model)
        {
            var register = await _warehouseServices.PostClientAsync("Warehouse/Create", model);
            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var roles = await _warehouseServices.GetAllClientsAsync("Warehouse/All");
            return Json(roles);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _warehouseServices.DeleteClientAsync($"Warehouse/Delete/{id}");
            return Json(deleted);
        }
        [HttpGet]
        public async Task<IActionResult> CheckDuplicate(string key, string val)
        {
            var usersResponse = await _warehouseServices.GetAllClientsAsync("Warehouse/All");
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
