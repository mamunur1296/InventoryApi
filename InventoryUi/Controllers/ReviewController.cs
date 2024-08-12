﻿using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryUi.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IClientServices<Review> _reviewServices;
        private readonly IUtilityHelper _utilityHelper;

        public ReviewController(IClientServices<Review> service, IUtilityHelper utilityHelper)
        {
            _reviewServices = service;
            _utilityHelper = utilityHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Review model)
        {
            var result = await _reviewServices.PostClientAsync("Review/Create", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var review = await _reviewServices.GetClientByIdAsync($"Review/get/{id}");
            return Json(review);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Review model)
        {
            var result = await _reviewServices.UpdateClientAsync($"Review/Update/{id}", model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var reviews = await _reviewServices.GetAllClientsAsync("Review/All");
            return Json(reviews);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _reviewServices.DeleteClientAsync($"Review/Delete/{id}");
            return Json(result);
        }
    }
}