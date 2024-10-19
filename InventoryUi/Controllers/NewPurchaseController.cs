using InventoryUi.DTOs;
using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class NewPurchaseController : Controller
    {
        private readonly IClientServices<Product> _productService;
        private readonly IClientServices<Supplier> _supplierServices;
        private readonly IClientServices<PurchaseItem> _purchaseServices;

        public NewPurchaseController(IClientServices<Product> productServices, IClientServices<Supplier> supplierServices, IClientServices<PurchaseItem> purchaseServices)
        {
            _productService = productServices;
            _supplierServices = supplierServices;
            _purchaseServices = purchaseServices;
        }
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SearchProducts(string term)
        {
            // Fetch all products from the service
            var products = await _productService.GetAllClientsAsync("Product/All");

            // Ensure the products data is not null
            if (products?.Data == null)
            {
                return Ok(new List<object>());
            }

            // Filter products based on the search term (ignoring case)
            var filteredProducts = products.Data
                .Where(p => p.ProductName.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(p => new
                {
                    label = p.ProductName, // Display name for the autocomplete dropdown
                    productid = p.Id       // Actual product ID
                })
                .ToList();

            // Return filtered products in the expected format for jQuery autocomplete
            return Ok(filteredProducts);
        }
        [HttpGet]
        public async Task<IActionResult> SearchSupplair(string term)
        {
            // Fetch all suppliers from the service
            var suppliers = await _supplierServices.GetAllClientsAsync("Supplier/All");

            // Ensure the suppliers data is not null
            if (suppliers?.Data == null)
            {
                return Ok(new List<object>());
            }

            // Filter suppliers based on the search term (ignoring case)
            var filteredSuppliers = suppliers.Data
                .Where(s => s.Phone.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(s => new
                {
                    label = s.SupplierName, // Display name for the autocomplete dropdown
                    id = s.Id,              // Actual supplier ID
                    phone = s.Phone         // Phone number for displaying details later
                })
                .ToList();

            // Return filtered suppliers in the expected format for jQuery autocomplete
            return Ok(filteredSuppliers);
        }

        [HttpPost]
        public async Task<IActionResult> Purchase([FromBody] PurchaseItem paymentItem)
        {
            if (paymentItem == null || paymentItem.Products == null || !paymentItem.Products.Any())
            {
                return BadRequest("Invalid purchase item data.");
            }

            var result = await _purchaseServices.PostClientAsync("Purchase/Purchase", paymentItem);
            if (result.Success)
            {
                return Json(true);
            }
            // Process the paymentItem here (e.g., save to database)

            return Json(false); // or return a specific result
        }


    }
}
