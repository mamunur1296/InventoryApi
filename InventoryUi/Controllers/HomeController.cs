using InventoryUi.Models;
using InventoryUi.Services.Interface;
using InventoryUi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InventoryUi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClientServices<Product> _productServices;

        public HomeController(ILogger<HomeController> logger, IClientServices<Product> productServices)
        {
            _logger = logger;
            _productServices = productServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Get8Product()
        {
            var allProducts = await _productServices.GetAllClientsAsync("Product/All");
            return Json(allProducts?.Data?.Take(8));
        }
        [HttpGet]
        public async Task<IActionResult> Shops(int page = 1, int pageSize = 10)
        {
            // Fetch all products
            var allProducts = await _productServices.GetAllClientsAsync("Product/All");
            if (allProducts.Success)
            {
                // Apply pagination
                var pagedProducts = allProducts?.Data?
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Calculate total pages
                var totalItems = allProducts?.Data?.Count();
                var totalPages = (int)Math.Ceiling((int)totalItems / (double)pageSize);

                // Create ViewModel
                var viewModel = new ProductListVm
                {
                    Products = pagedProducts,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalItems = (int)totalItems
                };

                return View(viewModel);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Serch()
        {
            var allProducts = await _productServices.GetAllClientsAsync("Product/All");
            // Create ViewModel
            var viewModel = new ProductListVm
            {
                Products = allProducts?.Data

            };
            return View(viewModel);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
