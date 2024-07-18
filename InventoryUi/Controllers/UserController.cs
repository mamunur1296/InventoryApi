using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventoryUi.Controllers
{
    public class UserController : Controller
    {
        private readonly IClientServices<User> _userServices;
        private readonly IFileUploader _fileUploder;

        public UserController(IClientServices<User> userServices, IFileUploader fileUploder)
        {
            _userServices = userServices;
            _fileUploder = fileUploder;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewData["ErrorMessage"] = "User ID is required.";
                return View(); // Return the view even if there's an error, to show the message
            }

            var user = await _userServices.GetClientByIdAsync($"User/{id}");

            if (user.Data == null)
            {
                ViewData["ErrorMessage"] = "User not found.";
                return View(); // Return the view even if there's an error, to show the message
            }

            return View(user.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Update(string id, User model)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewData["ErrorMessage"] = "User ID is required.";
                return View("Profile", model);
            }
            var user = await _userServices.GetClientByIdAsync($"User/{id}");
            if (model.FormFile != null)
            {
                if(user.Data.UserImg != null)
                {
                    bool deleteImg = await _fileUploder.DeleteFile(user.Data.UserImg);
                }
                model.UserImg = await _fileUploder.ImgUploader(model.FormFile);
            }
            else
            {
                model.UserImg = user.Data.UserImg;
            }
            model.Roles = new List<string> { "User" };
            var result = await _userServices.UpdateClientAsync($"User/Edit/{id}", model);

            if (result.Success)
            {
                // Update the claims in the authentication cookie
                var UpdatedUser = await _userServices.GetClientByIdAsync($"User/{id}");
                if (UpdatedUser.Data != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, UpdatedUser.Data.UserName),
                        new Claim("UserId", UpdatedUser.Data.Id.ToString()),
                        new Claim("FName", UpdatedUser.Data.FirstName),
                        new Claim("LName", UpdatedUser.Data.LastName),
                        new Claim("Email", UpdatedUser.Data.Email),
                        new Claim("Img", UpdatedUser.Data.UserImg ?? "https://via.placeholder.com/150")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                }
                // Redirect to the profile page with the user ID
                return RedirectToAction("Profile", new { id = id });
            }
            else
            {
                ViewData["ErrorMessage"] = "Error updating profile.";
                return View("Profile", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetallUser()
        {
            var users = await _userServices.GetAllClientsAsync("User/GetAll");
            return Json(new { data = users });
        }
    }
}
