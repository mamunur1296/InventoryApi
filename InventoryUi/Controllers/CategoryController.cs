using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IClientServices<Category> _categoryServices;
        private readonly IUtilityHelper _utilityHelper;

        public CategoryController(IClientServices<Category> service, IUtilityHelper utilityHelper)
        {
            _categoryServices = service;
            _utilityHelper = utilityHelper;
        }
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category model)
        {
            model.UpdatedBy = null;
            var result = await _categoryServices.PostClientAsync("Category/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var category = await _categoryServices.GetClientByIdAsync($"Category/get/{id}");
            return Json(category);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Category model)
        {
            model.CreatedBy = null;
            var result = await _categoryServices.UpdateClientAsync($"Category/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var categorys = await _categoryServices.GetAllClientsAsync("Category/All");
            return Json(categorys);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetallSubCatagory()
        {
            // Fetch all categories
            var categoriesResponse = await _categoryServices.GetAllClientsAsync("Category/All");

            // Check if data exists and filter subcategories (ParentCategoryID is not null or empty)
            if (categoriesResponse?.Data != null)
            {
                var subCategories = categoriesResponse.Data
                    .Where(c => !string.IsNullOrEmpty(c.ParentCategoryID))
                    .ToList();
                return Json(data : subCategories);
            }

            // Return an empty list or handle no subcategories scenario
            return Json(new List<Category>());
        }
        [HttpGet]
        public async Task<IActionResult> GetallCatagory()
        {
            // Fetch all categories
            var categoriesResponse = await _categoryServices.GetAllClientsAsync("Category/All");

            // Check if data exists and filter subcategories (ParentCategoryID is not null or empty)
            if (categoriesResponse?.Data != null)
            {
                var Categories = categoriesResponse.Data
                    .Where(c => string.IsNullOrEmpty(c.ParentCategoryID))
                    .ToList();
                return Json(data: Categories);
            }

            // Return an empty list or handle no subcategories scenario
            return Json(new List<Category>());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _categoryServices.DeleteClientAsync($"Category/Delete/{id}");
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> CheckDuplicate(string key, string val)
        {
            var categorys = await _categoryServices.GetAllClientsAsync("Category/All");
            if (categorys.Success)
            {
                return Json(await _utilityHelper.IsDuplicate(categorys?.Data, key, val));
            }
            return Json(false);
        }
    }
}
