using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
    public class ShiftController : Controller
    {
        private readonly IClientServices<Shift> _services;

        public ShiftController(IClientServices<Shift> services)
        {
            _services = services;
        }
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Shift model)
        {
            model.UpdatedBy = null;
            var Shift = await _services.PostClientAsync("Shift/Create", model);
            return Json(Shift);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var Shift = await _services.GetClientByIdAsync($"Shift/get/{id}");
            return Json(Shift);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Shift model)
        {
            model.CreatedBy = null;
            var result = await _services.UpdateClientAsync($"Shift/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Shift = await _services.GetAllClientsAsync("Shift/All");
            return Json(Shift);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _services.DeleteClientAsync($"Shift/Delete/{id}");
            return Json(result);
        }
    }
}
