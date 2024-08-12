using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class SupplierController : Controller
    {
        private readonly IClientServices<Supplier> _supplierServices;
        private readonly IUtilityHelper _utilityHelper;

        public SupplierController(IClientServices<Supplier> service, IUtilityHelper utilityHelper)
        {
            _supplierServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Supplier model)
        {
            var result = await _supplierServices.PostClientAsync("Supplier/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var supplier = await _supplierServices.GetClientByIdAsync($"Supplier/get/{id}");
            return Json(supplier);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Supplier model)
        {
            var result = await _supplierServices.UpdateClientAsync($"Supplier/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var suppliers = await _supplierServices.GetAllClientsAsync("Supplier/All");
            return Json(suppliers);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _supplierServices.DeleteClientAsync($"Supplier/Delete/{id}");
            return Json(result);
        }
    }
}
