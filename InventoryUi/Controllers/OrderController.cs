using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class OrderController : Controller
    {
        private readonly IClientServices<Order> _orderServices;
        private readonly IUtilityHelper _utilityHelper;

        public OrderController(IClientServices<Order> service, IUtilityHelper utilityHelper)
        {
            _orderServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Order model)
        {
            model.UpdatedBy = null;
            var result = await _orderServices.PostClientAsync("Order/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var order = await _orderServices.GetClientByIdAsync($"Order/get/{id}");
            return Json(order);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Order model)
        {
            model.CreatedBy = null;
            var result = await _orderServices.UpdateClientAsync($"Order/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var orders = await _orderServices.GetAllClientsAsync("Order/All");
            return Json(orders);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _orderServices.DeleteClientAsync($"Order/Delete/{id}");
            return Json(result);
        }
    }
}
