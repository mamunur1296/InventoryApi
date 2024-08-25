using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;


namespace InventoryUi.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IClientServices<Employee> _employeeServices;
        private readonly IImageProcessor<Employee> _imageProcessor;
        private readonly IUtilityHelper _utilityHelper;
        private readonly IFileUploader _fileUploder;
        public EmployeeController(IClientServices<Employee> service, IUtilityHelper utilityHelper, IImageProcessor<Employee> imageProcessor, IFileUploader fileUploder)
        {
            _employeeServices = service;
            _utilityHelper = utilityHelper;
            _imageProcessor = imageProcessor;
            _fileUploder = fileUploder;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create( Employee model)
        {
            model.UpdatedBy = null;
            model.Photo = new byte[0];
            if (model.Files != null && model.Files.Count > 0)
            {
                model.PhotoPath = await _fileUploder.ImgUploader(model.Files[0], "Employee");
            }
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
            model.CreatedBy = null;
            model.Photo = new byte[0];
            var employee = await _employeeServices.GetClientByIdAsync($"Employee/get/{id}");
            if (model.Files != null && model.Files.Count > 0)
            {
                if (employee.Data.PhotoPath != null)
                {
                     await _fileUploder.DeleteFile(employee.Data.PhotoPath, "Employee");
                }
                model.PhotoPath = await _fileUploder.ImgUploader(model.Files[0], "Employee");
            }
            else
            {
                model.PhotoPath = employee.Data.PhotoPath;
            }
            var result = await _employeeServices.UpdateClientAsync($"Employee/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var employees = await _employeeServices.GetAllClientsAsync("Employee/All");
            return Json(employees);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var employee = await _employeeServices.GetClientByIdAsync($"Employee/get/{id}");
            var result = await _employeeServices.DeleteClientAsync($"Employee/Delete/{id}");
            if (result.Success)
            {
                
                await _fileUploder.DeleteFile(employee.Data.PhotoPath, "Employee");
            }
            return Json(result);

        }
    }
}
