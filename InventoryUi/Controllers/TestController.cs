using InventoryUi.Models;
using InventoryUi.POC;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;


namespace InventoryUi.Controllers
{
    public class TestController : Controller
    {
        private readonly IClientServices<Menu> _menuServices;

        public TestController(IClientServices<Menu> menuServices)
        {
            _menuServices = menuServices;
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _menuServices.GetAllClientsAsync("Menu/All");

            var model = new MenuViewModel
            {
                Menus = menus.Data,
                NewMenu = new Menu()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(MenuViewModel model)
        {
            model.NewMenu.UpdatedBy = null;

            var result = await _menuServices.PostClientAsync("Menu/Create", model.NewMenu);

            if (result != null && result.Success)
            {
                var menus = await _menuServices.GetAllClientsAsync("Menu/All");

                // Return the updated product list as a partial view
                return PartialView("ProductList", menus.Data);
            }

            // If there was an error, add a model error to be displayed to the user
            ModelState.AddModelError(string.Empty, "Unable to create product. Please try again.");

            // Reload the existing menus to maintain consistency and return the partial view with errors
            model.Menus = (await _menuServices.GetAllClientsAsync("Menu/All")).Data;

            // Return the partial view with the model containing errors
            return PartialView("ProductList", model.Menus);
        }




    }
}
