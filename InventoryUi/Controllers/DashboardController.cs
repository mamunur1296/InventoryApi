using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
