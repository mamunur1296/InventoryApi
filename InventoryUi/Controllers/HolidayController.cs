using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class HolidayController : Controller
    {
        private readonly IClientServices<Holiday> _services;

        public HolidayController(IClientServices<Holiday> services)
        {
            _services = services;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Holiday model)
        {
            model.UpdatedBy = null;
            var Holiday = await _services.PostClientAsync("Holiday/Create", model);
            return Json(Holiday);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var Holiday = await _services.GetClientByIdAsync($"Holiday/get/{id}");
            return Json(Holiday);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Holiday model)
        {
            model.CreatedBy = null;
            var result = await _services.UpdateClientAsync($"Holiday/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Holiday = await _services.GetAllClientsAsync("Holiday/All");
            return Json(Holiday);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _services.DeleteClientAsync($"Holiday/Delete/{id}");
            return Json(result);
        }
    }
}
