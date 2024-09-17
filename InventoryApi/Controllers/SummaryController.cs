using InventoryApi.DataContext;
using InventoryApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;


namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummaryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SummaryController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("SalesSummary")]
        public async Task<IActionResult> SalesSummary()
        {
            var salesSummary = new SalesSummaryDTOs();

            // Define dates for filtering
            var today = DateTime.Today;
            var lastDay = today.AddDays(-1);
            var firstDayOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            var firstDayOfLastMonth = firstDayOfCurrentMonth.AddMonths(-1);
            var lastDayOfLastMonth = firstDayOfCurrentMonth.AddDays(-1);
            
            // Today Sales
            var todaySales = await _context.OrderDetails
                .Where(od => od.Order.OrderDate.Date == today)
                .GroupBy(od => 1)
                .Select(g => new
                {
                    TotalSalesAmount = g.Sum(od => (od.Quantity * od.UnitPrice) * (1 - (od.Discount / 100))), // Applying discount as percentage
                    TotalUnits = g.Sum(od => od.Quantity)
                }).FirstOrDefaultAsync();

            // Last Day Sales
            var lastDaySales = await _context.OrderDetails
                .Where(od => od.Order.OrderDate.Date == lastDay)
                .GroupBy(od => 1)
                .Select(g => new
                {
                    TotalSalesAmount = g.Sum(od => (od.Quantity * od.UnitPrice) * (1 - (od.Discount / 100))), // Applying discount as percentage
                    TotalUnits = g.Sum(od => od.Quantity)
                }).FirstOrDefaultAsync();

            // Current Month Sales
            var currentMonthSales = await _context.OrderDetails
                .Where(od => od.Order.OrderDate.Date >= firstDayOfCurrentMonth && od.Order.OrderDate.Date <= today)
                .GroupBy(od => 1)
                .Select(g => new
                {
                    TotalSalesAmount = g.Sum(od => (od.Quantity * od.UnitPrice) * (1 - (od.Discount / 100))), // Applying discount as percentage
                    TotalUnits = g.Sum(od => od.Quantity)
                }).FirstOrDefaultAsync();

            // Last Month Sales
            var lastMonthSales = await _context.OrderDetails
                .Where(od => od.Order.OrderDate.Date >= firstDayOfLastMonth && od.Order.OrderDate.Date <= lastDayOfLastMonth)
                .GroupBy(od => 1)
                .Select(g => new
                {
                    TotalSalesAmount = g.Sum(od => (od.Quantity * od.UnitPrice) * (1 - (od.Discount / 100))), // Applying discount as percentage
                    TotalUnits = g.Sum(od => od.Quantity)
                }).FirstOrDefaultAsync();

            // Populate DTO with results (use 0 as default if no sales found)
            salesSummary.TodaySalesAmount = todaySales?.TotalSalesAmount ?? 0;
            salesSummary.TodaySalesUnit = todaySales?.TotalUnits ?? 0;

            salesSummary.LastDaySalesAmount = lastDaySales?.TotalSalesAmount ?? 0;
            salesSummary.LastDaySalesUnit = lastDaySales?.TotalUnits ?? 0;

            salesSummary.CurrentMonthSalesAmount = currentMonthSales?.TotalSalesAmount ?? 0;
            salesSummary.CurrentMonthSalesUnit = currentMonthSales?.TotalUnits ?? 0;

            salesSummary.LastMonthSalesAmount = lastMonthSales?.TotalSalesAmount ?? 0;
            salesSummary.LastMonthSalesUnit = lastMonthSales?.TotalUnits ?? 0;

            // Return the summary result
            return StatusCode((int)HttpStatusCode.Created, new ResponseDTOs<SalesSummaryDTOs>
            {
                Success = true,
                Data = salesSummary,
                Status = HttpStatusCode.OK,
                Detail = "Samary  get   successfully !!."
            });
        }

     




    }
}
