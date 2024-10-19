using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class PurchaseDetailController : Controller
    {
        private readonly IClientServices<PurchaseDetail> _purchaseDetailServices;

        public PurchaseDetailController(IClientServices<PurchaseDetail> purchaseDetailServices)
        {
            _purchaseDetailServices = purchaseDetailServices;
        }
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PurchaseDetail model)
        {
            model.UpdatedBy = null;
            var result = await _purchaseDetailServices.PostClientAsync("PurchaseDetail/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var PurchaseDetail = await _purchaseDetailServices.GetClientByIdAsync($"PurchaseDetail/get/{id}");
            return Json(PurchaseDetail);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, PurchaseDetail model)
        {
            model.CreatedBy = null;
            var result = await _purchaseDetailServices.UpdateClientAsync($"PurchaseDetail/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var PurchaseDetail = await _purchaseDetailServices.GetAllClientsAsync("PurchaseDetail/All");
            return Json(PurchaseDetail);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _purchaseDetailServices.DeleteClientAsync($"PurchaseDetail/Delete/{id}");
            return Json(result);
        }
    }
}
