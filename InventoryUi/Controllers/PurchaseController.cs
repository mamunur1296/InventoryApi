using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
    public class PurchaseController : Controller
    {
        private readonly IClientServices<Purchase> _purchaseServices;

        public PurchaseController(IClientServices<Purchase> purchaseServices)
        {
            _purchaseServices = purchaseServices;
        }
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Purchase model)
        {
            model.UpdatedBy = null;
            var result = await _purchaseServices.PostClientAsync("Purchase/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var Purchase = await _purchaseServices.GetClientByIdAsync($"Purchase/get/{id}");
            return Json(Purchase);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Purchase model)
        {
            model.CreatedBy = null;
            var result = await _purchaseServices.UpdateClientAsync($"Purchase/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Purchase = await _purchaseServices.GetAllClientsAsync("Purchase/All");
            return Json(Purchase);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _purchaseServices.DeleteClientAsync($"Purchase/Delete/{id}");
            return Json(result);
        }
    }
}
