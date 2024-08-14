﻿using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IClientServices<Company> _companyServices;

        public CompanyController(IClientServices<Company> service)
        {
            _companyServices = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Company model)
        {
            var register = await _companyServices.PostClientAsync("Company/Create", model);
            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var roles = await _companyServices.GetAllClientsAsync("Company/All");
            return Json(roles);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var CompanyItem = await _companyServices.GetClientByIdAsync($"Company/get/{id}");
            return Json(CompanyItem);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Company model)
        {
            var result = await _companyServices.UpdateClientAsync($"Company/Update/{id}", model);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _companyServices.DeleteClientAsync($"Company/Delete/{id}");
            return Json(deleted);
        }
        [HttpGet]
        public async Task<IActionResult> CheckDuplicate(string key, string val)
        {
            var usersResponse = await _companyServices.GetAllClientsAsync("Company/All");
            if (usersResponse.Success)
            {
                bool isDuplicate = usersResponse.Data.Any(user =>
                {
                    var propertyInfo = user.GetType().GetProperty(key);
                    if (propertyInfo == null) return false;
                    var propertyValue = propertyInfo.GetValue(user, null)?.ToString();
                    return propertyValue?.Trim().Equals(val.Trim(), StringComparison.OrdinalIgnoreCase) ?? false;
                });

                return Json(isDuplicate);
            }
            return Json(false);
        }
    }
}
