using InventoryUi.Extensions;
using InventoryUi.Helpers;
using InventoryUi.Models;
using InventoryUi.Services.Interface;
using InventoryUi.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;





namespace InventoryUi.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
    public class NewOrderController : Controller
    {
        private readonly IClientServices<Product> _productServices;
        private readonly IClientServices<Customer> _customerServices;
        private readonly IClientServices<Category> _catagoryServices;
        private readonly IClientServices<Supplier> _supplairServices;
        private readonly IClientServices<UnitChild> _unitchildServices;
        private readonly IClientServices<UnitMaster> _unitMasterServices;
        private readonly IClientServices<Order> _orderService;
        private readonly IClientServices<NewOrderVm> _newOrderServices;
        private readonly IClientServices<Employee> _employeeServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IClientServices<OrderDetail> _orderDetailServices;
        public NewOrderController(
            IClientServices<Product> productServices,
            IClientServices<Customer> customerServices,
            IClientServices<Category> catagoryServices,
            IClientServices<Supplier> supplairServices,
            IClientServices<UnitChild> unitchildServices,
            IClientServices<UnitMaster> unitMasterServices,
            IClientServices<Order> orderService,
            IClientServices<NewOrderVm> newOrderServices,
            IWebHostEnvironment webHostEnvironment,
            IClientServices<OrderDetail> orderDetailServices,
            IClientServices<Employee> employeeServices

            )
        {
            _productServices = productServices;
            _customerServices = customerServices;
            _catagoryServices = catagoryServices;
            _supplairServices = supplairServices;
            _unitchildServices = unitchildServices;
            _unitMasterServices = unitMasterServices;
            _orderService = orderService;
            _newOrderServices = newOrderServices;
            _webHostEnvironment = webHostEnvironment;
            _orderDetailServices= orderDetailServices;
            _employeeServices = employeeServices;
        }

        [HttpGet]
        public async Task<IActionResult> SarchProduct()
       {
            var products = await _productServices.GetAllClientsAsync("Product/All");
            var name = HttpContext.Request.Query["productTerm"].ToString();

            var filteredProducts = products?.Data?
             .Where(p => p.ProductName.Contains(name)) // Add condition for Quantity
             .Select(p => new
             {
                 label = p.ProductName, // Display name
                 productid = p.Id       // Actual product ID
             })
             .ToList();

            return Ok(filteredProducts);
        }
        [HttpGet]
        public async Task<IActionResult> SearchCustomer() // Corrected method name
        {
            var customers = await _customerServices.GetAllClientsAsync("Customer/All");
            var name = HttpContext.Request.Query["CustomerTerm"].ToString();

            var filteredCustomers = customers?.Data?
                .Where(c => c.Phone.Contains(name))
                .Select(c => new
                {
                    label = c.Phone, // Display name
                    customerId = c.Id // Actual customer ID
                })
                .ToList();

            return Ok(filteredCustomers);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public async Task<IActionResult> Index(string searchTerm, bool isPartial = false)
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
            var categorys = await _catagoryServices.GetAllClientsAsync("Category/All");
            var suppliers = await _supplairServices.GetAllClientsAsync("Supplier/All");
            var unitChild = await _unitchildServices.GetAllClientsAsync("UnitChild/All");
            var unitMasters = await _unitMasterServices.GetAllClientsAsync("UnitMaster/All");

            // Prepare the view model
            var vm = new NewOrderVm
            {
                Products = products?.Data,
                Customers = customers?.Data,
                CategoryLIst = categorys?.Data,
                Suppliers = suppliers?.Data,
                unitChildrens= unitChild?.Data,
                unitMasters= unitMasters?.Data,
                ProductsListFromSession = productList,
                CustomerLIstFromSession = customer, // Pass customer to the view model
                IsPaymentButtonEnabled = productList.Any()  // Enable payment button if product list is not empty and customer is set
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

            // Return the appropriate view based on the isPartial parameter
            if (isPartial)
            {
                return PartialView("_ProductVawser", vm);
            }
            else
            {
                
                return View(vm); 
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                // Return an empty partial view if no productId is provided
                return PartialView("_ProductListPartial", new List<Product>());
            }

            var product = await _productServices.GetClientByIdAsync($"Product/get/{productId}");

            if (product?.Data == null)
            {
                // Return an empty partial view if the product is not found
                return PartialView("_ProductListPartial", new List<Product>());
            }
           
           
            var productList = HttpContext.Session.GetObject<List<Product>>("ProductList") ?? new List<Product>();

            var existingProduct = productList.FirstOrDefault(p => p.Id == product.Data.Id);
            if (product?.Data?.UnitsInStock <= existingProduct?.Quentity || product?.Data?.UnitsInStock <= 0)
            {
                return Json(new
                {
                    Success = false,
                    Message = $"Stock Error: Insufficient stock for '{product?.Data?.ProductName}'. Available stock: {product?.Data?.UnitsInStock ?? 0}"
                });
            }

            if (existingProduct == null)
            {
                product.Data.Quentity = 1;
                product.Data.Disc = 0;
                product.Data.TotalPriceWithoutDiscount = (int)(product.Data.UnitPrice * product.Data.Quentity);
                product.Data.TotalPrice = (int)(product.Data.UnitPrice * product.Data.Quentity * (1 - product.Data.Disc / 100));
                product.Data.TotlaDiscAmount = (int)(product.Data.UnitPrice * product.Data.Quentity) - product.Data.TotalPrice;

                productList.Add(product.Data);
            }
            else
            {
                existingProduct.Quentity += 1;
                existingProduct.TotalPriceWithoutDiscount = (int)(existingProduct.UnitPrice * existingProduct.Quentity);
                existingProduct.TotalPrice = (int)(existingProduct.UnitPrice * existingProduct.Quentity * (1 - existingProduct.Disc / 100));
                existingProduct.TotlaDiscAmount = (int)(existingProduct.UnitPrice * existingProduct.Quentity) - existingProduct.TotalPrice;
            }

            var discountAmount = productList.Sum(p => p.TotlaDiscAmount);
            var totalAmount = productList.Sum(p => p.TotalPriceWithoutDiscount);

            HttpContext.Session.SetInt32("DiscountAmount", discountAmount);
            HttpContext.Session.SetInt32("totalAmount", totalAmount);
            HttpContext.Session.SetObject("ProductList", productList);
            var vm = new NewOrderVm
            {
                ProductsListFromSession = productList,
            };
            return PartialView("_ProductListPartial", vm);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return PartialView("_ProductListPartial", new List<Product>());
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
            var vm = new NewOrderVm
            {
                ProductsListFromSession = productList,

            };
            return PartialView("_ProductListPartial", vm);

        }
        [HttpPost]
        public async Task<IActionResult> UpdateProductItem(string productId, int quantity, decimal discount)
        {
            var productList = HttpContext.Session.GetObject<List<Product>>("ProductList");
            var productDb = await _productServices.GetClientByIdAsync($"Product/get/{productId}");

            if (productDb?.Data?.UnitsInStock < quantity)
            {
                return Json( new
                {
                    Success = false,
                    Message = $"Stock Error: Insufficient stock for '{productDb.Data.ProductName}'. Available stock: {productDb.Data.UnitsInStock}"
                });
            }


            if (productList == null)
            {
                return PartialView("_ProductListPartial", new List<Product>());
            }

            var product = productList.FirstOrDefault(p => p.Id.ToString().Equals(productId, StringComparison.OrdinalIgnoreCase));
            if (product == null)
            {
                return PartialView("_ProductListPartial", new List<Product>());
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
            var vm = new NewOrderVm
            {
                ProductsListFromSession = productList,

            };
            return PartialView("_ProductListPartial", vm);
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

            return Json("Index");
        }
        public IActionResult Cancel()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public async Task<IActionResult> Payment()
        {
            var productList = HttpContext.Session.GetObject<List<Product>>("ProductList") ?? new List<Product>();
            var customer = HttpContext.Session.GetObject<Customer>("CurrentCustomer") ?? new Customer();
            var userId = string.Empty;
            var userName = string.Empty;
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    var userIdClaim = claimsIdentity.FindFirst("UserId") ?? claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                    if (userIdClaim != null)
                    {
                        userId = userIdClaim.Value;
                    }
                    var userNameClaim = claimsIdentity.FindFirst(ClaimTypes.Name);
                    if (userNameClaim != null)
                    {
                        userName = userNameClaim.Value;
                    }
                }
            }
            var model = new NewOrderVm
            {
                ProductsListFromSession = productList,
                CustomerLIstFromSession = customer,
                EmployeeId = userId,
                CreatedBy= userName,
            };
            model.CustomerLIstFromSession.CustomerName = customer?.CustomerName ?? "No Name";
            model.CustomerLIstFromSession.Phone = customer?.Phone ?? "null";
            model.CustomerLIstFromSession.PasswordHash = "password";
            string uniqueInvoiceNumber = InvoiceHelper.GenerateInvoiceNumber();
            model.InvoiceNumber = uniqueInvoiceNumber;
            var result = await _newOrderServices.PostClientAsync("OrderMaping/Create", model);
            if (result.Success)
            {
                return Json(new
                {
                    Success = true,
                    Message = result.Data,
                });
            }
            else
            {
                // Return a JSON object with the error details
                return Json(new
                {
                    Success = false,
                    Message = result.Detail
                });
            }

        }
    }
}

