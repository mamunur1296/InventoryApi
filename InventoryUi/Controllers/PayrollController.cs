using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class PayrollController : Controller
    {
        private readonly IClientServices<Payroll> _services;

        public PayrollController(IClientServices<Payroll> services)
        {
            _services = services;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Payroll model)
        {
            model.UpdatedBy = null;
            var Payroll = await _services.PostClientAsync("Payroll/Create", model);
            return Json(Payroll);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var Payroll = await _services.GetClientByIdAsync($"Payroll/get/{id}");
            return Json(Payroll);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Payroll model)
        {
            model.CreatedBy = null;
            var result = await _services.UpdateClientAsync($"Payroll/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Payroll = await _services.GetAllClientsAsync("Payroll/All");
            return Json(Payroll);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _services.DeleteClientAsync($"Payroll/Delete/{id}");
            return Json(result);
        }
    }
}
