using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class SubMenuController : Controller
    {
        private readonly IClientServices<SubMenu> _subMenuServices;
        private readonly IUtilityHelper _utilityHelper;

        public SubMenuController(IClientServices<SubMenu> service, IUtilityHelper utilityHelper)
        {
            _subMenuServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SubMenu model)
        {
            var result = await _subMenuServices.PostClientAsync("SubMenu/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var subMenu = await _subMenuServices.GetClientByIdAsync($"SubMenu/get/{id}");
            return Json(subMenu);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, SubMenu model)
        {
            var result = await _subMenuServices.UpdateClientAsync($"SubMenu/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var subMenus = await _subMenuServices.GetAllClientsAsync("SubMenu/All");
            return Json(subMenus);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _subMenuServices.DeleteClientAsync($"SubMenu/Delete/{id}");
            return Json(result);
        }
    }
}
