using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
    public class CustomerController : Controller
    {
        private readonly IClientServices<Customer> _customerServices;
        private readonly IUtilityHelper _utilityHelper;

        public CustomerController(IClientServices<Customer> service, IUtilityHelper utilityHelper)
        {
            _customerServices = service;
            _utilityHelper = utilityHelper;
        }
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Customer model)
        {
            model.UpdatedBy = null;
            model.PasswordHash = "PasswordHash";
            var result = await _customerServices.PostClientAsync("Customer/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> CreateCustomer(string id)
        {
            var result = await _customerServices.GetClientByIdAsync($"Customer/CreateByAdmin/{id}");
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
            model.CreatedBy = null;
            var result = await _customerServices.UpdateClientAsync($"Customer/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var customers = await _customerServices.GetAllClientsAsync("Customer/All");
            return Json(customers);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _customerServices.DeleteClientAsync($"Customer/Delete/{id}");
            return Json(result);
        }
    }
}
