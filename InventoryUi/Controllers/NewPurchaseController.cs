using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class NewPurchaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
