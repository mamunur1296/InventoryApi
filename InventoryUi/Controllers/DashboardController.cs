using InventoryUi.Models;
using InventoryUi.Services.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InventoryUi.Controllers
{
   
    public class DashboardController : Controller
    {
        private readonly IClientServices<User> _userServices;
        private readonly IClientServices<Company> _companyServices;
        private readonly IClientServices<Login> _loginServices;
        private readonly ITokenService _tokenService;

        public DashboardController(IClientServices<User> userServices, IClientServices<Company> companyServices, IClientServices<Login> loginServices, ITokenService tokenService)
        {
            _userServices = userServices;
            _companyServices = companyServices;
            _loginServices = loginServices;
            _tokenService = tokenService;
        }
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public async Task<IActionResult> Index()
        {
            return View(); // Still returning DashboirdVm
        }

        [HttpGet]
        public IActionResult Login(string? ReturnUrl = null)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("AuthSchemeDashboard");
            _tokenService.ClearToken();
            return RedirectToAction("Login");
        }
        [HttpPost]
        public async Task<IActionResult> LoginUser(Login model, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = ReturnUrl;
                return View("Login", model);
            }

            var loginResponse = await _loginServices.Login("Auth/Login", model);

            if (loginResponse?.Data?.token == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                ViewData["ReturnUrl"] = ReturnUrl;
                return View("Login", model);
            }

            _tokenService.SaveToken(loginResponse.Data.token);
            // Extract role from JWT token before UserLogin
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(loginResponse.Data.token);

            // Extract the roles from the token claims
            var roleClaims = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).ToList();
            string roleName = roleClaims.FirstOrDefault();
            await DashboardUserLogin(loginResponse.Data.token);

            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            if (roleName == "User")
            {
                TempData["LoginMessage"] = "Access denied. This login page is restricted to administrators only.";
                return RedirectToAction("Login", "Dashboard");
            }

            return RedirectToAction("Index", "Dashboard");

        }
        private async Task DashboardUserLogin(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var identity = new ClaimsIdentity("AuthSchemeDashboard");

            foreach (var claim in jwt.Claims)
            {
                identity.AddClaim(claim);
            }

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("AuthSchemeDashboard", principal);

            // Log user details if needed
            var userId = jwt.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var userName = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var roles = jwt.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "AuthSchemeDashboard")]
        public async Task<IActionResult> GetNotApprovedEmployees()
        {
            var users = await _userServices.GetAllClientsAsync("User/GetAll");
            if (users?.Data != null)
            {
                var result = users.Data
                    .Where(item => item.isEmployee && !item.isApprovedByAdmin)
                    .ToList();
                return Json(result);
            }
            return Json(null);
        }
        
        

    }
}
