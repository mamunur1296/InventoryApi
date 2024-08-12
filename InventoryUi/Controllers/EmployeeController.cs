using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

namespace InventoryUi.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IClientServices<Employee> _employeeServices;
        private readonly IUtilityHelper _utilityHelper;

        public EmployeeController(IClientServices<Employee> service, IUtilityHelper utilityHelper)
        {
            _employeeServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Employee model)
        {
            model.Photo = new byte[0];
            model.PhotoPath = "https://www.example.com/image.jpg";
            var result = await _employeeServices.PostClientAsync("Employee/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var employee = await _employeeServices.GetClientByIdAsync($"Employee/get/{id}");
            return Json(employee);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Employee model)
        {
            model.Photo = new byte[0];
            model.PhotoPath= "https://www.example.com/image.jpg";
            var result = await _employeeServices.UpdateClientAsync($"Employee/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var employees = await _employeeServices.GetAllClientsAsync("Employee/All");
            return Json(employees);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _employeeServices.DeleteClientAsync($"Employee/Delete/{id}");
            return Json(result);
        }
    }
}
