using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class PointOfPurchaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
