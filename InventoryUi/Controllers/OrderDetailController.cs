using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly IClientServices<OrderDetail> _orderDetailServices;
        private readonly IUtilityHelper _utilityHelper;

        public OrderDetailController(IClientServices<OrderDetail> service, IUtilityHelper utilityHelper)
        {
            _orderDetailServices = service;
            _utilityHelper = utilityHelper;
        }
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrderDetail model)
        {
            model.UpdatedBy = null;
            var result = await _orderDetailServices.PostClientAsync("OrderDetail/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var orderDetail = await _orderDetailServices.GetClientByIdAsync($"OrderDetail/get/{id}");
            return Json(orderDetail);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, OrderDetail model)
        {
            model.CreatedBy = null;
            var result = await _orderDetailServices.UpdateClientAsync($"OrderDetail/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var orderDetails = await _orderDetailServices.GetAllClientsAsync("OrderDetail/All");
            return Json(orderDetails);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _orderDetailServices.DeleteClientAsync($"OrderDetail/Delete/{id}");
            return Json(result);
        }
    }
}
