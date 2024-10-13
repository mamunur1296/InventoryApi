using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IClientServices<Department> _services;

        public DepartmentController(IClientServices<Department> services)
        {
            _services = services;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department model)
        {
            model.UpdatedBy = null;
            var Department = await _services.PostClientAsync("Department/Create", model);
            return Json(Department);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var Department = await _services.GetClientByIdAsync($"Department/get/{id}");
            return Json(Department);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Department model)
        {
            model.CreatedBy = null;
            var result = await _services.UpdateClientAsync($"Department/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Department = await _services.GetAllClientsAsync("Department/All");
            return Json(Department);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _services.DeleteClientAsync($"Department/Delete/{id}");
            return Json(result);
        }
    }
}
