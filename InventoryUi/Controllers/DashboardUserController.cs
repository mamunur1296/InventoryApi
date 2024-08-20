using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventoryUi.Controllers
{
    [Authorize]
    public class DashboardUserController : Controller
    {
        private readonly IClientServices<User> _userServices;
        private readonly IClientServices<Register> _registerServices;
        private readonly IFileUploader _fileUploder;

        public DashboardUserController(IClientServices<User> userServices, IClientServices<Register> registerServices, IFileUploader fileUploder)
        {
            _userServices = userServices;
            _registerServices = registerServices;
            _fileUploder = fileUploder;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var users = await _userServices.GetAllClientsAsync("User/GetAll");
            return Json(users);
        }
        [HttpGet]
        public async Task<IActionResult> CheckDuplicate(string key, string val)
        {
            var usersResponse = await _userServices.GetAllClientsAsync("User/GetAll");
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
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userServices.GetClientByIdAsync($"User/{id}");
            return Json(user);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Register model)
        {
            // Initialize the Roles list with the RoleName value
            model.Roles = new List<string> { model.RoleName };
            var register = await _registerServices.PostClientAsync("Auth/Register", model);
            return Json(register);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _userServices.DeleteClientAsync($"User/{id}");
            return Json(deleted);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, User model)
        {
            var user = await _userServices.GetClientByIdAsync($"User/{id}");
            if (model.FormFile != null)
            {
                if (user.Data.UserImg != null)
                {
                    bool deleteImg = await _fileUploder.DeleteFile(user.Data.UserImg);
                }
                model.UserImg = await _fileUploder.ImgUploader(model?.FormFile);
            }
            else
            {
                model.UserImg = user.Data.UserImg;
            }
            if (model.Roles == null)
            {
                model.Roles = user.Data.Roles;
            }
            var result = await _userServices.UpdateClientAsync($"User/Edit/{id}", model);

            if (result.Success)
            {
                // Update the claims in the authentication cookie
                var UpdatedUser = await _userServices.GetClientByIdAsync($"User/{id}");
                if (UpdatedUser.Data != null)
                {
                    var loggedInUserId = User.FindFirst("UserId")?.Value;
                    if (id == loggedInUserId)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, UpdatedUser.Data.UserName ?? string.Empty),
                            new Claim("UserId", UpdatedUser.Data.Id.ToString() ?? string.Empty),
                            new Claim("FName", UpdatedUser.Data.FirstName ?? string.Empty),
                            new Claim("LName", UpdatedUser.Data.LastName ?? string.Empty),
                            new Claim("Email", UpdatedUser.Data.Email ?? string.Empty ),
                            new Claim("Img", UpdatedUser?.Data?.UserImg ?? string.Empty)
                        };
                        // Add roles to the claims
                        if (UpdatedUser.Data.Roles != null)
                        {
                            foreach (var role in UpdatedUser.Data.Roles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role));
                            }
                        }
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    }
                    
                }
                // Redirect to the profile page with the user ID
            }
             return Json(result);
        }

    }
}