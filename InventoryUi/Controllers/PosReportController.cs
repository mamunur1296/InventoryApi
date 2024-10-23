using AspNetCore.Reporting;
using InventoryUi.Helpers;
using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
    public class PosReportController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IClientServices<Order> _orderServices;
        private readonly IClientServices<OrderDetail> _orderDetailServices;
        private readonly IClientServices<Customer> _customerServices;
        private readonly IClientServices<Employee> _employeeServices;
        private readonly IClientServices<Product> _productServices;

    
        public PosReportController(
            IWebHostEnvironment webHostEnvironment, 
            IClientServices<Order> orderServices,
            IClientServices<OrderDetail> orderDetailServices,
            IClientServices<Customer> customerServices,
            IClientServices<Employee> employeeServices,
            IClientServices<Product> productServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _orderServices = orderServices;
            _orderDetailServices = orderDetailServices;
            _customerServices = customerServices;
            _employeeServices = employeeServices;
            _productServices = productServices;
        }
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> DownloadInvoice(string id , bool isDownload = false)
        {
            string mimeType = "application/pdf";

            try
            {
                // Fetch the necessary data (consider refactoring to get data specific to the order)
                var order = await _orderServices.GetClientByIdAsync($"Order/get/{id}");
                if (order == null || order.Data == null) return NotFound("Order not found.");

                var orderDetails = await _orderDetailServices.GetAllClientsAsync("OrderDetail/All");
                if (orderDetails == null || orderDetails.Data == null) return NotFound("Order details not found.");

                var products = await _productServices.GetAllClientsAsync("Product/All");
                var customers = await _customerServices.GetAllClientsAsync("Customer/All");
                var employees = await _employeeServices.GetAllClientsAsync("Employee/All");

                // Extract customer, employee, and other order-related information
                var customer = customers?.Data?.FirstOrDefault(c => c.Id.ToString() == order?.Data?.CustomerID?.ToString());
                var employee = employees?.Data?.FirstOrDefault(e => e.Id.ToString() == order?.Data?.EmployeeID?.ToString());

               

                // Filter the order details for the specific order
                var orderDetailList = orderDetails.Data
                    .Where(o => o.OrderID.ToString() == order.Data.Id.ToString())
                    .ToList();
                order.Data.OrderDetails = orderDetailList;

                // Calculate Subtotal and prepare the order details
                var ReportOrderDetail = orderDetailList.Select(od => new
                {
                    OrderID = od.OrderID,
                    ProductID = od.ProductID,
                    ProductName = products?.Data?.FirstOrDefault(p => p.Id.ToString() == od?.ProductID?.ToString())?.ProductName,
                    UnitPrice = Math.Round(od.UnitPrice, 2),  // Ensure UnitPrice has 2 decimal places
                    Quantity = od.Quantity,
                    Discount = od.Discount,
                    TotalPrice = Math.Round(od.Quantity * od.UnitPrice * (1 - od.Discount / 100), 2) // Calculate total per line with 2 decimals
                }).ToList();

                // Calculate subtotal and create the ReportOrder object
                var subtotal = Math.Round(ReportOrderDetail.Sum(item => item.TotalPrice), 2); // Ensure Subtotal has 2 decimal places
                var deliveryCharge = 50.00;  // Assuming delivery charge is fixed
                var total = Math.Round((double)subtotal + (double)deliveryCharge, 2);  // Ensure Total has 2 decimal places
                var ReportOrder = new
                {
                    OrderID = order.Data.Id,
                    CustomerName = customer?.CustomerName,
                    CustomerPhone = customer?.Phone,
                    EmployeeName = employee?.FirstName,
                    EmployeePhone = employee?.HomePhone,
                    CreationDate = order.Data.CreationDate,
                    CustomerAddress = customer?.Address,
                    Subtotal = subtotal.ToString("F2"), // Format subtotal as string with 2 decimal places
                    DalivaryCharge = deliveryCharge.ToString("F2"), // Format delivery charge with 2 decimals
                    Total = total.ToString("F2"), // Format total as string with 2 decimal places
                    Paid = total.ToString("F2"),  // Assuming the paid amount equals the total
                    TodayDate = DateTime.Now.ToString("MM/dd/yy"), // Format the current date as MM/dd/yy
                    CurrentTime = DateTime.Now.ToString("hh:mm:ss tt"),
                    InvoiceNumber=order.Data.InvoiceNumber,
                };


                // Convert to DataTables
                var dtOrderDetails = dtHelpers.ListToDt(ReportOrderDetail);
                var dtOrder = dtHelpers.ObjectToDataTable(ReportOrder);

                // Set up the report path using the WebRootPath
                string reportPath = Path.Combine(_webHostEnvironment.WebRootPath, "Repots", "Invoice.rdlc");

                // Check if the report file exists
                if (!System.IO.File.Exists(reportPath))
                {
                    return NotFound("Report file not found.");
                }

                // Create the local report with the correct path
                var localReport = new LocalReport(reportPath);

                // Add the data source to the report
                localReport.AddDataSource("OrdereDetailsList", dtOrderDetails);
                localReport.AddDataSource("Order", dtOrder);

                // Render the report as a PDF
                var result = localReport.Execute(RenderType.Pdf, 1, null, mimeType);
                if (isDownload)
                {
                    // Return the PDF file
                    return File(result.MainStream.ToArray(), mimeType, "Invoice.pdf");
                }
                else
                { 
                    return File(result.MainStream.ToArray(), mimeType);
                }

            }
            catch (Exception ex)
            {
                // Log the exception for debugging
           
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
