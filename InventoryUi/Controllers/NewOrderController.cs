using InventoryUi.Extensions;
using InventoryUi.Models;
using InventoryUi.Services.Interface;
using InventoryUi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace InventoryUi.Controllers
{
    public class NewOrderController : Controller
    {
        private readonly IClientServices<Product> _productServices;
        private readonly IClientServices<Customer> _customerServices;
        private readonly IClientServices<Order> _orderService;
        private readonly IClientServices<NewOrderVm> _newOrderServices;

        public NewOrderController(IClientServices<Product> productServices, IClientServices<Customer> customerServices, IClientServices<Order> orderService, IClientServices<NewOrderVm> newOrderServices)
        {
            _productServices = productServices;
            _customerServices = customerServices;
            _orderService = orderService;
            _newOrderServices = newOrderServices;
        }
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            var products = await _productServices.GetAllClientsAsync("Product/All");
            var name = HttpContext.Request.Query["term"].ToString();

            var filteredProducts = products?.Data?
                .Where(c => c.ProductName.Contains(name))
                .Select(c => new
                {
                    label = c.ProductName, // Display name
                    productid = c.Id    // Actual product ID
                })
                .ToList();

            return Ok(filteredProducts);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm)
        {
            // Retrieve the list of products from the session or initialize a new list
            var productList = HttpContext.Session.GetObject<List<Product>>("ProductList") ?? new List<Product>();

            // Retrieve the current customer from the session or initialize a new customer
            var customer = HttpContext.Session.GetObject<Customer>("CurrentCustomer") ?? new Customer();

            // Retrieve discount amount and total amount from session with default values
            var discountAmount = HttpContext.Session.GetInt32("DiscountAmount") ?? 0;
            var totalAmount = HttpContext.Session.GetInt32("totalAmount") ?? 0;

            // Fetch product and customer lists from the services
            var products = await _productServices.GetAllClientsAsync("Product/All");
            var customers = await _customerServices.GetAllClientsAsync("Customer/All");

            // Prepare the view model
            var vm = new NewOrderVm
            {
                Products = products?.Data,
                Customers = customers?.Data,
                ProductsListFromSession = productList,
                CustomerLIstFromSession = customer, // Pass customer to the view model
                IsPaymentButtonEnabled = productList.Any() && customer.CustomerName != null  // Enable payment button if product list is not empty and customer is set
            };
            // Update customer properties
            customer.totalAmount = totalAmount;
            customer.productDiscountedTotal = totalAmount + discountAmount;
            customer.DiscountedAmount = discountAmount;
            customer.SubTotal = customer.productDiscountedTotal - discountAmount;
            customer.PaymentAmount = customer.SubTotal - customer.vat; // Ensure vat is set or initialized
            customer.FynalyPaymentAmount = customer.PaymentAmount - customer.DueAmount;

            // Update session with the modified customer object
            HttpContext.Session.SetObject("CurrentCustomer", customer);

            // Return the view with the view model
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return RedirectToAction("Index");
            }

            var product = await _productServices.GetClientByIdAsync($"Product/get/{productId}");

            if (product?.Data != null)
            {
                var productList = HttpContext.Session.GetObject<List<Product>>("ProductList") ?? new List<Product>();

                // Check if the product is already in the cart
                var existingProduct = productList.FirstOrDefault(p => p.Id == product.Data.Id);
                if (existingProduct == null)
                {
                    // Initialize new product quantities and discounts
                    product.Data.Quentity = 1; // Default quantity
                    product.Data.Disc = 0; // Default discount
                    product.Data.TotalPriceWithoutDiscount = (int)(product.Data.UnitPrice * product.Data.Quentity);
                    product.Data.TotalPrice = (int)(product.Data.UnitPrice * product.Data.Quentity * (1 - product.Data.Disc / 100));
                    product.Data.TotlaDiscAmount = (int)(product.Data.UnitPrice * product.Data.Quentity) - product.Data.TotalPrice;

                    // Add product to the list
                    productList.Add(product.Data);
                }
                else
                {
                    // Update existing product quantities and discounts
                    existingProduct.Quentity += 1;
                    existingProduct.TotalPriceWithoutDiscount = (int)(existingProduct.UnitPrice * existingProduct.Quentity);
                    existingProduct.TotalPrice = (int)(existingProduct.UnitPrice * existingProduct.Quentity * (1 - existingProduct.Disc / 100));
                    existingProduct.TotlaDiscAmount = (int)(existingProduct.UnitPrice * existingProduct.Quentity) - existingProduct.TotalPrice;
                }

                // Calculate and update discountAmount and totalAmount
                var discountAmount = HttpContext.Session.GetInt32("DiscountAmount") ?? 0;
                var totalAmount = HttpContext.Session.GetInt32("totalAmount") ?? 0;

                // Recalculate discountAmount and totalAmount
                discountAmount = productList.Sum(p => p.TotlaDiscAmount);
                totalAmount = productList.Sum(p => p.TotalPriceWithoutDiscount);

                HttpContext.Session.SetInt32("DiscountAmount", discountAmount);
                HttpContext.Session.SetInt32("totalAmount", totalAmount);
                HttpContext.Session.SetObject("ProductList", productList);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return RedirectToAction("Index");
            }

            var productList = HttpContext.Session.GetObject<List<Product>>("ProductList");

            if (productList != null)
            {
                var product = productList.FirstOrDefault(p => p.Id.ToString().Equals(productId, StringComparison.OrdinalIgnoreCase));

                if (product != null)
                {
                    productList.Remove(product);
                    var discountAmount = HttpContext.Session.GetInt32("DiscountAmount") ?? 0;
                    var totalAmount = HttpContext.Session.GetInt32("totalAmount") ?? 0;
                    discountAmount -= product.TotlaDiscAmount;
                    totalAmount -= product.TotalPrice;
                    HttpContext.Session.SetInt32("DiscountAmount", discountAmount);
                    HttpContext.Session.SetInt32("totalAmount", totalAmount);
                    HttpContext.Session.SetObject("ProductList", productList);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(string productId, int quantity, decimal discount)
        {
            var productList = HttpContext.Session.GetObject<List<Product>>("ProductList");
            if (productList == null)
            {
                return RedirectToAction("Index");
            }

            var product = productList.FirstOrDefault(p => p.Id.ToString().Equals(productId, StringComparison.OrdinalIgnoreCase));
            if (product == null)
            {
                return RedirectToAction("Index");
            }

            // Calculate the previous total price and discount amount for the product
            var previousTotalPrice = (int)(product.UnitPrice * product.Quentity * (1 - product.Disc / 100));
            var previousDiscountAmount = (int)(product.UnitPrice * product.Quentity) - previousTotalPrice;

            // Update product details
            product.Quentity = quantity;
            product.Disc = discount;
            product.TotalPriceWithoutDiscount = (int)(product.UnitPrice * product.Quentity);
            product.TotalPrice = (int)(product.UnitPrice * product.Quentity * (1 - product.Disc / 100));
            product.TotlaDiscAmount = product.TotalPriceWithoutDiscount - product.TotalPrice;

            // Recalculate discountAmount and totalAmount for all products
            int discountAmount = 0;
            int totalAmount = 0;

            foreach (var prod in productList)
            {
                totalAmount += (int)(prod.UnitPrice * prod.Quentity * (1 - prod.Disc / 100));
                discountAmount += (int)(prod.UnitPrice * prod.Quentity) - (int)(prod.UnitPrice * prod.Quentity * (1 - prod.Disc / 100));
            }

            // Save updated values in the session
            HttpContext.Session.SetInt32("DiscountAmount", discountAmount);
            HttpContext.Session.SetInt32("totalAmount", totalAmount);
            HttpContext.Session.SetObject("ProductList", productList);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                return RedirectToAction("Index");
            }

            var customer = await _customerServices.GetClientByIdAsync($"Customer/get/{customerId}");

            if (customer?.Data != null)
            {
                HttpContext.Session.SetObject("CurrentCustomer", customer.Data);
            }

            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult>  Payment()
        {
            var productList = HttpContext.Session.GetObject<List<Product>>("ProductList") ?? new List<Product>();
            var customer = HttpContext.Session.GetObject<Customer>("CurrentCustomer") ?? new Customer();
            // Prepare the view model
            var model = new NewOrderVm
            {
                ProductsListFromSession = productList,
                CustomerLIstFromSession = customer,
            }; 
            var result = await _newOrderServices.PostClientAsync("OrderMaping/Create", model);
            return RedirectToAction("Index");
        }

        public IActionResult Cancel()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
