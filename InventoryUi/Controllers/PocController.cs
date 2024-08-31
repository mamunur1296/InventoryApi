using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class PocController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
       
    }
}
