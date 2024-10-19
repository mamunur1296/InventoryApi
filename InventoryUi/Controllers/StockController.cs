using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class StockController : Controller
    {
        private readonly IClientServices<Stock> _stockServices;
        private readonly IUtilityHelper _utilityHelper;

        public StockController(IClientServices<Stock> service, IUtilityHelper utilityHelper)
        {
            _stockServices = service;
            _utilityHelper = utilityHelper;
        }
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Stock model)
        {
            model.UpdatedBy = null;
            var result = await _stockServices.PostClientAsync("Stock/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var stock = await _stockServices.GetClientByIdAsync($"Stock/get/{id}");
            return Json(stock);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Stock model)
        {
            model.CreatedBy = null;
            var result = await _stockServices.UpdateClientAsync($"Stock/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var stocka = await _stockServices.GetAllClientsAsync("Stock/All");
            return Json(stocka);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _stockServices.DeleteClientAsync($"Stock/Delete/{id}");
            return Json(result);
        }
    }
}
