using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IClientServices<Attendance> _services;

        public AttendanceController(IClientServices<Attendance> services)
        {
            _services = services;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Attendance model)
        {
            model.UpdatedBy = null;
            var Attendance = await _services.PostClientAsync("Attendance/Create", model);
            return Json(Attendance);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var Attendance = await _services.GetClientByIdAsync($"Attendance/get/{id}");
            return Json(Attendance);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Attendance model)
        {
            model.CreatedBy = null;
            var result = await _services.UpdateClientAsync($"Attendance/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Attendance = await _services.GetAllClientsAsync("Attendance/All");
            return Json(Attendance);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _services.DeleteClientAsync($"Attendance/Delete/{id}");
            return Json(result);
        }
    }
}
