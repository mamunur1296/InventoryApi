using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class ShipperController : Controller
    {
        private readonly IClientServices<Shipper> _shipperServices;
        private readonly IUtilityHelper _utilityHelper;

        public ShipperController(IClientServices<Shipper> service, IUtilityHelper utilityHelper)
        {
            _shipperServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Shipper model)
        {
            model.UpdatedBy = null;
            var result = await _shipperServices.PostClientAsync("Shipper/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var shipper = await _shipperServices.GetClientByIdAsync($"Shipper/get/{id}");
            return Json(shipper);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Shipper model)
        {
            model.CreatedBy = null;
            var result = await _shipperServices.UpdateClientAsync($"Shipper/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var shippers = await _shipperServices.GetAllClientsAsync("Shipper/All");
            return Json(shippers);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _shipperServices.DeleteClientAsync($"Shipper/Delete/{id}");
            return Json(result);
        }
    }
}
