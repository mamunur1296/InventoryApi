using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class UnitMasterController : Controller
    {
        private readonly IClientServices<UnitMaster> _unitMasterServices;
        private readonly IUtilityHelper _utilityHelper;

        public UnitMasterController(IClientServices<UnitMaster> service, IUtilityHelper utilityHelper)
        {
            _unitMasterServices = service;
            _utilityHelper = utilityHelper;
        }
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UnitMaster model)
        {
            model.UpdatedBy = null;
            var result = await _unitMasterServices.PostClientAsync("UnitMaster/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var unitMaster = await _unitMasterServices.GetClientByIdAsync($"UnitMaster/get/{id}");
            return Json(unitMaster);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, UnitMaster model)
        {
            model.CreatedBy = null;
            var result = await _unitMasterServices.UpdateClientAsync($"UnitMaster/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var unitMaster = await _unitMasterServices.GetAllClientsAsync("UnitMaster/All");
            return Json(unitMaster);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _unitMasterServices.DeleteClientAsync($"UnitMaster/Delete/{id}");
            return Json(result);
        }
    }
}
