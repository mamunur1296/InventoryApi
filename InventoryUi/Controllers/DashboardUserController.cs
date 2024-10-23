using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using InventoryUi.ViewModel;

namespace InventoryUi.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
    public class DashboardUserController : Controller
    {
        private readonly IClientServices<User> _userServices;
        private readonly IClientServices<Register> _registerServices;
        private readonly IClientServices<Company> _companyServices;
        private readonly IClientServices<Branch> _branchServices;
        private readonly IFileUploader _fileUploder;

        public DashboardUserController(IClientServices<User> userServices,
            IClientServices<Company> companyServices,
            IClientServices<Branch> branchServices,
            IClientServices<Register> registerServices, IFileUploader fileUploder)
        {
            _userServices = userServices;
            _registerServices = registerServices;
            _fileUploder = fileUploder;
            _companyServices= companyServices;
            _branchServices = branchServices;
        }
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public async Task<IActionResult> Index()
        {
            var companys = await _companyServices.GetAllClientsAsync("Company/All");
            var branch = await _branchServices.GetAllClientsAsync("Branch/All");
            var vm = new DashboirdUserVm();
            if (companys.Success)
            {
                if (companys.Data.Any())
                {
                    vm.Company = companys.Data.First();
                }
                else
                {
                    // Handle the case when there are no companies
                    vm.Company = null; // Or set to a default value as needed
                }
            }
            vm.Branches = branch.Data;
            return View(vm);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public async Task<IActionResult> GetaCompanyForEmpPartial()
        {
            var companys = await _companyServices.GetAllClientsAsync("Company/All");
            var branch = await _branchServices.GetAllClientsAsync("Branch/All");
            var vm = new DashboirdUserVm();
            if (companys.Success)
            {
                if (companys.Data.Any())
                {
                    vm.Company = companys.Data.First();
                }
                else
                {
                    // Handle the case when there are no companies
                    vm.Company = null; // Or set to a default value as needed
                }
                vm.Branches = branch.Data;
                return PartialView("_isEmployeeSection", vm); // Fetch fresh data
            }
            return NotFound();
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public async Task<IActionResult> Getall()
        {
            var users = await _userServices.GetAllClientsAsync("User/GetAll");
            return Json(users);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
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
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userServices.GetClientByIdAsync($"User/{id}");
            var vm = new DashboirdUserVm();
            if (user.Success)
            {
                vm.User = user.Data;
            }
            return Json(vm);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public async Task<IActionResult> Create(Register model)
        {
            // Initialize the Roles list with the RoleName value
            model.Roles = new List<string> { model.RoleName };
            var register = await _registerServices.PostClientAsync("Auth/Register", model);
            return Json(register);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public async Task<IActionResult> Register([FromForm] DashboirdUserVm model)
        {
            // Initialize the Roles list with the RoleName value
            model.User.Roles = new List<string> { model.User.RoleName };
            var register = await _userServices.PostClientAsync("Auth/Register", model.User);
            return Json(register);
        }
        [HttpDelete]
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _userServices.DeleteClientAsync($"User/{id}");
            return Json(deleted);
        }
        [HttpPut]
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public async Task<IActionResult> Update(string id, DashboirdUserVm model)
        {
            var user = await _userServices.GetClientByIdAsync($"User/{id}");
            if (model.User.FormFile != null)
            {
                if (user.Data.UserImg != null)
                {
                    bool deleteImg = await _fileUploder.DeleteFile(user.Data.UserImg,"User");
                }
                model.User.UserImg = await _fileUploder.ImgUploader(model?.User?.FormFile , "User");
            }
            else
            {
                model.User.UserImg = user.Data.UserImg;
            }
            if (model.User.RoleName == null)
            {
                model.User.Roles = user.Data.Roles;
            }
            else
            {
                model.User.Roles = new List<string> { model.User.RoleName };
            }
            var result = await _userServices.UpdateClientAsync($"User/Edit/{id}", model.User);

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