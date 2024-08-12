﻿using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class DeliveryAddressController : Controller
    {
        private readonly IClientServices<DeliveryAddress> _deliveryAddressServices;
        private readonly IUtilityHelper _utilityHelper;

        public DeliveryAddressController(IClientServices<DeliveryAddress> service, IUtilityHelper utilityHelper)
        {
            _deliveryAddressServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DeliveryAddress model)
        {
            var result = await _deliveryAddressServices.PostClientAsync("DeliveryAddress/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var deliveryAddres = await _deliveryAddressServices.GetClientByIdAsync($"DeliveryAddress/get/{id}");
            return Json(deliveryAddres);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, DeliveryAddress model)
        {
            var result = await _deliveryAddressServices.UpdateClientAsync($"DeliveryAddress/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var deliveryAddress = await _deliveryAddressServices.GetAllClientsAsync("DeliveryAddress/All");
            return Json(deliveryAddress);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _deliveryAddressServices.DeleteClientAsync($"DeliveryAddress/Delete/{id}");
            return Json(result);
        }
    }
}
