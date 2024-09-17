using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;

namespace InventoryUi.Controllers
{
    public class OrderController : Controller
    {
        private readonly IClientServices<Order> _orderServices;
        private readonly IUtilityHelper _utilityHelper;

        public OrderController(IClientServices<Order> service, IUtilityHelper utilityHelper)
        {
            _orderServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Order model)
        {
            model.UpdatedBy = null;
            var result = await _orderServices.PostClientAsync("Order/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var order = await _orderServices.GetClientByIdAsync($"Order/get/{id}");
            return Json(order);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Order model)
        {
            model.CreatedBy = null;
            var result = await _orderServices.UpdateClientAsync($"Order/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var orders = await _orderServices.GetAllClientsAsync("Order/All");
            return Json(orders);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _orderServices.DeleteClientAsync($"Order/Delete/{id}");
            return Json(result);
        }
        
        public async Task<IActionResult> ReportForCsv()
        {
            // Create a DataTable and add some data (or fetch from your DB)
            var orders = await _orderServices.GetAllClientsAsync("Order/All");
            DataTable dataTable = ConvertToDataTable(orders.Data.ToList());

            // Convert DataTable to CSV string
            string csvContent = DataTableToCSV(dataTable);

            // Return the CSV file as a downloadable response
            var bytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
            var result = new FileContentResult(bytes, "text/csv")
            {
                FileDownloadName = "Reports.csv"
            };

            return result;
        }
        private DataTable ConvertToDataTable<T>(List<T> data)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            // Get all public properties of the type T
            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            // Add columns to the DataTable based on the properties of T
            foreach (var prop in properties)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            // Add rows to the DataTable
            foreach (var item in data)
            {
                var values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        private string DataTableToCSV(DataTable dataTable)
        {
            StringBuilder csvData = new StringBuilder();

            // Add the column names
            IEnumerable<string> columnNames = dataTable.Columns.Cast<DataColumn>()
                .Select(column => column.ColumnName);
            csvData.AppendLine(string.Join(",", columnNames));

            // Add the rows
            foreach (DataRow row in dataTable.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                csvData.AppendLine(string.Join(",", fields));
            }

            return csvData.ToString();
        }
    }
}
