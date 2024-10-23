using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
    public class BranchController : Controller
    {
        private readonly IClientServices<Branch> _branchServices;
        private readonly IUtilityHelper _utilityHelper;
        
        public BranchController(IClientServices<Branch> service, IUtilityHelper utilityHelper)
        {
            _branchServices = service;
            _utilityHelper = utilityHelper;
        }
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Branch model)
        {
            model.UpdatedBy = null;
            var result = await _branchServices.PostClientAsync("Branch/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var branch = await _branchServices.GetClientByIdAsync($"Branch/get/{id}");
            return Json(branch);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Branch model)
        {
            model.CreatedBy = null;
            var result = await _branchServices.UpdateClientAsync($"Branch/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var branch = await _branchServices.GetAllClientsAsync("Branch/All");
            return Json(branch);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _branchServices.DeleteClientAsync($"Branch/Delete/{id}");
            return Json(result);
        }
        
    }
}
