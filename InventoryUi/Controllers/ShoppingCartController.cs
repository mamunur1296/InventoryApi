using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IClientServices<ShoppingCart> _shoppingCartServices;
        private readonly IUtilityHelper _utilityHelper;

        public ShoppingCartController(IClientServices<ShoppingCart> service, IUtilityHelper utilityHelper)
        {
            _shoppingCartServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ShoppingCart model)
        {
            model.UpdatedBy = null;
            var result = await _shoppingCartServices.PostClientAsync("ShoppingCart/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var shoppingCart = await _shoppingCartServices.GetClientByIdAsync($"ShoppingCart/get/{id}");
            return Json(shoppingCart);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, ShoppingCart model)
        {
            model.CreatedBy = null;
            var result = await _shoppingCartServices.UpdateClientAsync($"ShoppingCart/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var shoppingCarts = await _shoppingCartServices.GetAllClientsAsync("ShoppingCart/All");
            return Json(shoppingCarts);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _shoppingCartServices.DeleteClientAsync($"ShoppingCart/Delete/{id}");
            return Json(result);
        }
    }
}
