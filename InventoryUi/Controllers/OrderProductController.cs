using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class OrderProductController : Controller
    {
        private readonly IClientServices<OrderProduct> _orderProductServices;
        private readonly IUtilityHelper _utilityHelper;

        public OrderProductController(IClientServices<OrderProduct> service, IUtilityHelper utilityHelper)
        {
            _orderProductServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrderProduct model)
        {
            model.UpdatedBy = null;
            var result = await _orderProductServices.PostClientAsync("OrderProduct/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var orderProduct = await _orderProductServices.GetClientByIdAsync($"OrderProduct/get/{id}");
            return Json(orderProduct);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, OrderProduct model)
        {
            model.CreatedBy = null;
            var result = await _orderProductServices.UpdateClientAsync($"OrderProduct/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var orderProducts = await _orderProductServices.GetAllClientsAsync("OrderProduct/All");
            return Json(orderProducts);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _orderProductServices.DeleteClientAsync($"OrderProduct/Delete/{id}");
            return Json(result);
        }
    }
}
