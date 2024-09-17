using InventoryApi.DTOs;
using InventoryApi.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using InventoryApi.DataContext;
using InventoryApi.Services.Interfaces;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderMapingController : ControllerBase
    {
        
        private readonly ApplicationDbContext _context;
        private readonly IBaseServices<EmployeeDTOs> _service;

        // Ensure only one constructor with required dependencies
        public OrderMapingController( ApplicationDbContext context , IBaseServices<EmployeeDTOs> services)
        {
            
            _context = context;
            _service = services;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(NewOrderDTOs model)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDTOs<string>
                {
                    Success = false,
                    Status = HttpStatusCode.BadRequest,
                    Detail = "Invalid order data provided."
                });
            }
            var isEmployee = await _context.Employees.FindAsync(model.EmployeeId);
            if (isEmployee == null)
            {
                return NotFound(new ResponseDTOs<string>
                {
                    Success = false,
                    Status = HttpStatusCode.NotFound,
                    Detail = "Authorization Error: The provided Employee  does not exist. Please verify the employee details and try again."
                });
            }


            using (var transaction = await _context.Database.BeginTransactionAsync()) // Start transaction
            {
                try
                {
                    // Create new order
                    var customerID = model.CustomerLIstFromSession?.Id;
                    var newOrder = new Order
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreatedBy = model.CreatedBy,
                        CreationDate = DateTime.Now,
                        EmployeeID = model.EmployeeId,
                        OrderDate = DateTime.Now,
                        RequiredDate = DateTime.Now,
                        ShippedDate = DateTime.Now,
                        InvoiceNumber=model.InvoiceNumber,
                        IsHold=model.IsHold,
                        HoldReason=model.HoldReason,
                        ShipVia = 1,
                        Freight = 1,
                        ShipName = "mamun",
                        ShipAddress = "Bogura",
                        ShipCity = "Bogura",
                        ShipRegion = "Bogura",
                        ShipPostalCode = "5800",
                        ShipCountry = "Bogura",
                        PaymentStatus = "Paid",
                        OrderStatus = "Online",
                        CustomerID = !string.IsNullOrEmpty(customerID) && customerID != Guid.Empty.ToString() ? customerID : null,
                    };

                    await _context.Orders.AddAsync(newOrder);
                    await _context.SaveChangesAsync();

                    // Create order details
                    foreach (var product in model.ProductsListFromSession)
                    {
                        var orderDetails = new OrderDetail
                        {
                            Id = Guid.NewGuid().ToString(),
                            OrderID = newOrder.Id,
                            ProductID = product.Id,
                            UnitPrice = product.UnitPrice,
                            Quantity = product.Quentity,
                            Discount = product.Disc
                        };
                        
                        await _context.OrderDetails.AddAsync(orderDetails);
                        var existingProduct = await _context.Products.FindAsync(product.Id);
                        if (existingProduct != null)
                        {
                            existingProduct.UnitsInStock -= product.Quentity;
                            _context.Products.Update(existingProduct);
                        }
                    }
                    
                    // Commit the transaction after successful operations
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return StatusCode((int)HttpStatusCode.Created, new ResponseDTOs<string>
                    {
                        Success = true,
                        Status = HttpStatusCode.Created,
                        Detail = "Order created successfully.",
                        Data= newOrder.Id,

                    });
                }
                catch (DbUpdateException ex)
                {
                    // Rollback the transaction in case of any errors
                    await transaction.RollbackAsync();

                    // Handle specific errors, such as duplicate key violations
                    return Conflict(new ResponseDTOs<string>
                    {
                        Success = false,
                        Status = HttpStatusCode.Conflict,
                        Detail = $"Save Failed: {ex.Message}"
                    });
                }
                catch (Exception ex)
                {
                    // Rollback in case of any unexpected error
                    await transaction.RollbackAsync();

                    // Return general server error
                    return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseDTOs<string>
                    {
                        Success = false,
                        Status = HttpStatusCode.InternalServerError,
                        Detail = $"An unexpected error occurred: {ex.Message}"
                    });
                }
            }
        }
    }
}
