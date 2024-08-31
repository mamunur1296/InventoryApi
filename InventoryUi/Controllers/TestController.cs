using InventoryUi.POC;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class TestController : Controller
    {
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Apple", Price = 1.20m, Stock = 50 },
            new Product { Id = 2, Name = "Banana", Price = 0.80m, Stock = 100 },
            new Product { Id = 3, Name = "Orange", Price = 1.00m, Stock = 75 },
            new Product { Id = 4, Name = "Mango", Price = 1.50m, Stock = 20 },
            new Product { Id = 5, Name = "Grapes", Price = 2.00m, Stock = 60 }
        };

        public ActionResult Index()
        {
            return View();
        }

        // API to get product suggestions
        [HttpGet]
        public IActionResult GetProductSuggestions(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return Json(new List<Product>());
            }

            var suggestedProducts = _products
                .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p.Name)
                .ToList();

            return Json(suggestedProducts);
        }

        // POST: Add product to cart
        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            // Here you would normally add the product to the user's cart
            // For simplicity, we're just returning a success message

            var product = _products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                // Add to cart logic here (e.g., update session, database, etc.)
                return Json(new { success = true, message = $"{product.Name} added to cart." });
            }

            return Json(new { success = false, message = "Product not found." });
        }

    }
}
