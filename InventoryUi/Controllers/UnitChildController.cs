using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class UnitChildController : Controller
    {
        private readonly IClientServices<UnitChild> _unitChildServices;
        private readonly IClientServices<UnitMaster> _unitMasterServices;
        private readonly IUtilityHelper _utilityHelper;

        public UnitChildController(IClientServices<UnitChild> service, IUtilityHelper utilityHelper, IClientServices<UnitMaster> unitMasterServices)
        {
            _unitChildServices = service;
            _utilityHelper = utilityHelper;
            _unitMasterServices = unitMasterServices;
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
        [HttpGet]
        public async Task<IActionResult> GetallByFilterMaster(string id)
        {
            // Fetch all unit children
            var unitChild = await _unitChildServices.GetAllClientsAsync("UnitChild/All");

            // Fetch the specific UnitMaster by id
            var unitMaster = await _unitMasterServices.GetClientByIdAsync($"UnitMaster/get/{id}");

            // Filter unit children where the UnitMasterId matches the UnitMaster's Id
            var filterByMaster = unitChild?.Data?
                .Where(c => c.UnitMasterId.ToString() == unitMaster?.Data?.Id.ToString());

            return Json(filterByMaster);
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _unitChildServices.DeleteClientAsync($"UnitChild/Delete/{id}");
            return Json(result);
        }
       
    }
}
