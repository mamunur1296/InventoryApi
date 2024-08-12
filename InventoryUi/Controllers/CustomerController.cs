using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IClientServices<Customer> _customerServices;
        private readonly IUtilityHelper _utilityHelper;

        public CustomerController(IClientServices<Customer> service, IUtilityHelper utilityHelper)
        {
            _customerServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Customer model)
        {
            var result = await _customerServices.PostClientAsync("Customer/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var customer = await _customerServices.GetClientByIdAsync($"Customer/get/{id}");
            return Json(customer);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Customer model)
        {
            var result = await _customerServices.UpdateClientAsync($"Customer/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var customers = await _customerServices.GetAllClientsAsync("Customer/All");
            return Json(customers);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _customerServices.DeleteClientAsync($"Customer/Delete/{id}");
            return Json(result);
        }
    }
}
