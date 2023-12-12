using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using To_Do_List.API.Models;
using To_Do_List.Web.Models;

namespace To_Do_List.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly JWT _jwt;
        private readonly IToDoApi _api;

        public HomeController(ILogger<HomeController> logger, JWT jwt, IToDoApi api)
        {
            _logger = logger;
            _jwt = jwt;
            _api = api;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[HttpPost]
        //public async Task<IActionResult> Login([FromForm] UserDTO request)
        //{
        //    var token = await _api.Login(request);
        //    return View(token);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Index([FromForm]UserDTO request)
        //{
        //    var content = JsonSerializer.Serialize(request);
        //    StringContent stringContent = new StringContent(content, Encoding.UTF8, "application/json");
        //    HttpResponseMessage response = await _httpClient.PostAsync("login", stringContent);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return Error();
        //}
    }
}
