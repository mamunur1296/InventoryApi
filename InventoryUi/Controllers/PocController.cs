using InventoryUi.DTOs;
using InventoryUi.Services.Interface;
using InventoryUi.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class PocController : Controller
    {
        private readonly IClientServices<SalesSummary> _salesSummaryServices;

        public PocController(IClientServices<SalesSummary> salesSummaryServices)
        {
            _salesSummaryServices = salesSummaryServices;
        }
        [HttpGet]
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
