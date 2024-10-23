using InventoryUi.DTOs;
using InventoryUi.Services.Interface;
using InventoryUi.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
    public class PocController : Controller
    {
        private readonly IClientServices<SalesSummary> _salesSummaryServices;

        public PocController(IClientServices<SalesSummary> salesSummaryServices)
        {
            _salesSummaryServices = salesSummaryServices;
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public async Task<IActionResult> Index()
        {
            var vm = new SalesSummaryVm();
            var samary = await _salesSummaryServices.GetClientByIdAsync("Summary/SalesSummary");
            if(samary.Success)
            {
                vm.SalesSummary = samary.Data;
            }
            return View(vm);
        }
       
    }
}
