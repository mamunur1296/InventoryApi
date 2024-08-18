using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class CartItemController : Controller
    {
        private readonly IClientServices<CartItem> _cartItemServices;
        private readonly IUtilityHelper _utilityHelper;

        public CartItemController(IClientServices<CartItem> service, IUtilityHelper utilityHelper)
        {
            _cartItemServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CartItem model)
        {
            model.UpdatedBy = null;
            var result = await _cartItemServices.PostClientAsync("CartItem/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var cartItem = await _cartItemServices.GetClientByIdAsync($"CartItem/get/{id}");
            return Json(cartItem);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, CartItem model)
        {
            model.CreatedBy = null;
            var result = await _cartItemServices.UpdateClientAsync($"CartItem/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var cartItems = await _cartItemServices.GetAllClientsAsync("CartItem/All");
            return Json(cartItems);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _cartItemServices.DeleteClientAsync($"CartItem/Delete/{id}");
            return Json(result);
        }
    }
}
