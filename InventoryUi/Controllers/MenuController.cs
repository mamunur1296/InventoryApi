using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class MenuController : Controller
    {
        private readonly IClientServices<Menu> _menuServices;
        private readonly IUtilityHelper _utilityHelper;

        public MenuController(IClientServices<Menu> service, IUtilityHelper utilityHelper)
        {
            _menuServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Menu model)
        {
            model.UpdatedBy = null;
            var result = await _menuServices.PostClientAsync("Menu/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var menu = await _menuServices.GetClientByIdAsync($"Menu/get/{id}");
            return Json(menu);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Menu model)
        {
            model.CreatedBy = null;
            var result = await _menuServices.UpdateClientAsync($"Menu/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var menus = await _menuServices.GetAllClientsAsync("Menu/All");
            return Json(menus);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _menuServices.DeleteClientAsync($"Menu/Delete/{id}");
            return Json(result);
        }
    }
}
