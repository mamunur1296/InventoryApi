using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IClientServices<Payment> _paymentServices;
        private readonly IUtilityHelper _utilityHelper;

        public PaymentController(IClientServices<Payment> service, IUtilityHelper utilityHelper)
        {
            _paymentServices = service;
            _utilityHelper = utilityHelper;
        }
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Payment model)
        {
            model.UpdatedBy = null;
            var result = await _paymentServices.PostClientAsync("Payment/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var payment = await _paymentServices.GetClientByIdAsync($"Payment/get/{id}");
            return Json(payment);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Payment model)
        {
            model.CreatedBy = null;
            var result = await _paymentServices.UpdateClientAsync($"Payment/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var payments = await _paymentServices.GetAllClientsAsync("Payment/All");
            return Json(payments);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _paymentServices.DeleteClientAsync($"Payment/Delete/{id}");
            return Json(result);
        }
    }
}
