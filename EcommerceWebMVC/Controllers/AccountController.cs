// Controllers/AccountController.cs
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EcommerceLib.Dtos; // Đảm bảo bạn import namespace của Dtos

namespace AccountController.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            ViewData["HideSidebar"] = false; // Hiển thị sidebar ở trang khác
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                // Redirect to Home IF logged already
                return RedirectToAction("Index", "Home");
            }
            ViewData["HideSidebar"] = true;
            return View(new UserLoginDto());
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            ViewData["HideSidebar"] = true;
            if (!ModelState.IsValid)
            {
                ViewData["ShowValidationErrors"] = true; // validation errors while submit
                return View(model);
            }

            var response = await _httpClient.PostAsJsonAsync("http://localhost:5203/api/account/login", model);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                var userName = model.Username;
                HttpContext.Session.SetString("Token", token);//save to session
                HttpContext.Session.SetString("UserName", userName);//save to session
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Token");
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login");
        }
    }
}
