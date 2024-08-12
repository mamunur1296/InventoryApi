using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly IClientServices<Prescription> _prescriptionServices;
        private readonly IUtilityHelper _utilityHelper;

        public PrescriptionController(IClientServices<Prescription> service, IUtilityHelper utilityHelper)
        {
            _prescriptionServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Prescription model)
        {
            var result = await _prescriptionServices.PostClientAsync("Prescription/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var prescription = await _prescriptionServices.GetClientByIdAsync($"Prescription/get/{id}");
            return Json(prescription);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Prescription model)
        {
            var result = await _prescriptionServices.UpdateClientAsync($"Prescription/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var prescriptions = await _prescriptionServices.GetAllClientsAsync("Prescription/All");
            return Json(prescriptions);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _prescriptionServices.DeleteClientAsync($"Prescription/Delete/{id}");
            return Json(result);
        }
    }
}
