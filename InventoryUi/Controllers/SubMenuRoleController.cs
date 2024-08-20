using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class SubMenuRoleController : Controller
    {
        private readonly IClientServices<SubMenuRole> _subMenuRoleServices;
        private readonly IUtilityHelper _utilityHelper;

        public SubMenuRoleController(IClientServices<SubMenuRole> service, IUtilityHelper utilityHelper)
        {
            _subMenuRoleServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SubMenuRole model)
        {
            var result = await _subMenuRoleServices.PostClientAsync("SubMenuRole/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var subMenuRole = await _subMenuRoleServices.GetClientByIdAsync($"SubMenuRole/get/{id}");
            return Json(subMenuRole);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, SubMenuRole model)
        {
            var result = await _subMenuRoleServices.UpdateClientAsync($"SubMenuRole/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var subMenuRoles = await _subMenuRoleServices.GetAllClientsAsync("SubMenuRole/All");
            return Json(subMenuRoles);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _subMenuRoleServices.DeleteClientAsync($"SubMenuRole/Delete/{id}");
            return Json(result);
        }
    }
}
