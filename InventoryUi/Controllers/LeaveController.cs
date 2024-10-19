using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class LeaveController : Controller
    {
        private readonly IClientServices<Leave> _services;

        public LeaveController(IClientServices<Leave> services)
        {
            _services = services;
        }
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Leave model)
        {
            model.UpdatedBy = null;
            var Leave = await _services.PostClientAsync("Leave/Create", model);
            return Json(Leave);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var Leave = await _services.GetClientByIdAsync($"Leave/get/{id}");
            return Json(Leave);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Leave model)
        {
            model.CreatedBy = null;
            var result = await _services.UpdateClientAsync($"Leave/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Leave = await _services.GetAllClientsAsync("Leave/All");
            return Json(Leave);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _services.DeleteClientAsync($"Leave/Delete/{id}");
            return Json(result);
        }
    }
}
