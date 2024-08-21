﻿using InventoryUi.Models;
using InventoryUi.Services.Implemettions;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace InventoryUi.Controllers
{
    public class ProductController : Controller
    {
        private readonly IClientServices<Product> _productServices;
        private readonly IUtilityHelper _utilityHelper;
        private readonly IFileUploader _fileUploader;

        public ProductController(IClientServices<Product> service, IUtilityHelper utilityHelper, IFileUploader fileUploader)
        {
            _productServices = service;
            _utilityHelper = utilityHelper;
            _fileUploader = fileUploader;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product model)
        {
            model.UpdatedBy = null;
            if (model.Files != null && model.Files.Count > 0)
            {
                model.ImageURL = await _fileUploader.ImgUploader(model.Files[0], "Product");
            }
            var result = await _productServices.PostClientAsync("Product/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _productServices.GetClientByIdAsync($"Product/get/{id}");
            return Json(product);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Product model)
        {
            model.CreatedBy = null;
            var product = await _productServices.GetClientByIdAsync($"Product/get/{id}");
            if (model.Files != null && model.Files.Count > 0)
            {
                if (product?.Data?.ImageURL != null)
                {
                    await _fileUploader.DeleteFile(product.Data.ImageURL, "Product");
                }
                model.ImageURL = await _fileUploader.ImgUploader(model.Files[0], "Product");
            }
            else
            {
                model.ImageURL = product.Data.ImageURL;
            }
            var result = await _productServices.UpdateClientAsync($"Product/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var products = await _productServices.GetAllClientsAsync("Product/All");
            return Json(products);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _productServices.GetClientByIdAsync($"Product/get/{id}");
            var result = await _productServices.DeleteClientAsync($"Product/Delete/{id}");
            if (result.Success)
            {
                await _fileUploader.DeleteFile(product?.Data?.ImageURL, "Product");
            }
            return Json(result);
        }
    }
}
