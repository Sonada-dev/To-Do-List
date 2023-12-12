using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using To_Do_List.API.Models;
using To_Do_List.Web.Models;

namespace To_Do_List.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IToDoApi _api;

        public AccountController(IToDoApi api) => _api = api;

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] UserDTO request)
        {
            if (ModelState.IsValid)
            {
                var token = await _api.Login(request);
                if(token == null)
                    return View(request);

                Response.Cookies.Append("JwtToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                return RedirectToAction("Index", "Tasks");
            }

            return View(request);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] UserDTO request)
        {
            if(ModelState.IsValid)
            {
                var response = await _api.Register(request);
                return RedirectToAction("Login", "Account");
            }

            return View(request);
        }
        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("JwtToken");

            return RedirectToAction("Index", "Home");
        }
    }
}
