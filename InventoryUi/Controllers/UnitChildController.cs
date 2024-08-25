using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class UnitChildController : Controller
    {
        private readonly IClientServices<UnitChild> _unitChildServices;
        private readonly IUtilityHelper _utilityHelper;

        public UnitChildController(IClientServices<UnitChild> service, IUtilityHelper utilityHelper)
        {
            _unitChildServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UnitChild model)
        {
            model.UpdatedBy = null;
            var result = await _unitChildServices.PostClientAsync("UnitChild/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var unitChild = await _unitChildServices.GetClientByIdAsync($"UnitChild/get/{id}");
            return Json(unitChild);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, UnitChild model)
        {
            model.CreatedBy = null;
            var result = await _unitChildServices.UpdateClientAsync($"UnitChild/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var unitChild = await _unitChildServices.GetAllClientsAsync("UnitChild/All");
            return Json(unitChild);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _unitChildServices.DeleteClientAsync($"UnitChild/Delete/{id}");
            return Json(result);
        }
       
    }
}
