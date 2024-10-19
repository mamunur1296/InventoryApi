using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Supplier model)
        {
            model.UpdatedBy = null;
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
            model.CreatedBy = null;
            var result = await _supplierServices.UpdateClientAsync($"Supplier/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var suppliers = await _supplierServices.GetAllClientsAsync("Supplier/All");
            return Json(suppliers);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _supplierServices.DeleteClientAsync($"Supplier/Delete/{id}");
            return Json(result);
        }
    }
}
