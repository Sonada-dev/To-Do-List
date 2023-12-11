using Microsoft.AspNetCore.Mvc;
using Refit;
using RefitInterface;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using To_Do_List.API.Models;
using To_Do_List.Web.Models;

namespace To_Do_List.Web.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly IToDoApi _api;
        private readonly JWT _jwt;

        public AuthorizeController(IToDoApi api, JWT jwt, IHttpClientFactory httpClientFactory)
        {
            _api = api;
            _jwt = jwt;
        }

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

                Response.Cookies.Append("JwtToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                _jwt.Token = token;

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
            if(!ModelState.IsValid)
            {
                var response = await _api.Register(request);
                return RedirectToAction("Login", "Authorize");
            }

            return View(request);
        }
    }
}
