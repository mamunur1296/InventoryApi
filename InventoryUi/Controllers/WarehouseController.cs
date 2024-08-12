using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly IClientServices<Warehouse> _warehouseServices;
        private readonly IUtilityHelper _utilityHelper;

        public WarehouseController(IClientServices<Warehouse> service, IUtilityHelper utilityHelper)
        {
            _warehouseServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Warehouse model)
        {
            var result = await _warehouseServices.PostClientAsync("Warehouse/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var warehouse = await _warehouseServices.GetClientByIdAsync($"Warehouse/get/{id}");
            return Json(warehouse);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Warehouse model)
        {
            var result = await _warehouseServices.UpdateClientAsync($"Warehouse/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var warehouses = await _warehouseServices.GetAllClientsAsync("Warehouse/All");
            return Json(warehouses);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _warehouseServices.DeleteClientAsync($"Warehouse/Delete/{id}");
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> CheckDuplicate(string key, string val)
        {
            var warehouses = await _warehouseServices.GetAllClientsAsync("Warehouse/All");
            if (warehouses.Success)
            {
                return Json(await _utilityHelper.IsDuplicate(warehouses?.Data, key, val));
            }
            return Json(false);
        }
    }
}
