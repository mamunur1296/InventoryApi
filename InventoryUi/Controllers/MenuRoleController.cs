using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class MenuRoleController : Controller
    {
        private readonly IClientServices<MenuRole> _menuRoleServices;
        private readonly IUtilityHelper _utilityHelper;

        public MenuRoleController(IClientServices<MenuRole> service, IUtilityHelper utilityHelper)
        {
            _menuRoleServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MenuRole model)
        {

            var result = await _menuRoleServices.PostClientAsync("MenuRole/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var menuRole = await _menuRoleServices.GetClientByIdAsync($"MenuRole/get/{id}");
            return Json(menuRole);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, MenuRole model)
        {
            
            var result = await _menuRoleServices.UpdateClientAsync($"MenuRole/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var menuRoles = await _menuRoleServices.GetAllClientsAsync("MenuRole/All");
            return Json(menuRoles);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _menuRoleServices.DeleteClientAsync($"MenuRole/Delete/{id}");
            return Json(result);
        }
    }
}
