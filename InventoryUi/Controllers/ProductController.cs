using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class ProductController : Controller
    {
        private readonly IClientServices<Product> _productServices;
        private readonly IUtilityHelper _utilityHelper;

        public ProductController(IClientServices<Product> service, IUtilityHelper utilityHelper)
        {
            _productServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product model)
        {
            var result = await _productServices.PostClientAsync("Product/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _productServices.GetClientByIdAsync($"Product/get/{id}");
            return Json(product);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Product model)
        {
            var result = await _productServices.UpdateClientAsync($"Product/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var products = await _productServices.GetAllClientsAsync("Product/All");
            return Json(products);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _productServices.DeleteClientAsync($"Product/Delete/{id}");
            return Json(result);
        }
    }
}
