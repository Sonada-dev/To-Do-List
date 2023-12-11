using Microsoft.AspNetCore.Mvc;
using Refit;
using RefitInterface;
using System.Text;
using System.Text.Json;
using To_Do_List.API.Models;
using To_Do_List.Web.Models;

namespace To_Do_List.Web.Controllers
{
    public class AuthorizeController : Controller
    {
        //private readonly IHttpClientFactory _httpClientFactory;
        //private readonly HttpClient _httpClient;
        private readonly IToDoApi _api;
        private readonly JWT _jwt;

        public AuthorizeController(/*IHttpClientFactory httpClientFactory,*/ IToDoApi api, JWT jwt)
        {
            //_httpClientFactory = httpClientFactory;
            //_httpClient = _httpClientFactory.CreateClient("Api");
            _api = api;
            _jwt = jwt;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] UserDTO request)
        {
            if (ModelState.IsValid)
            {
                var token = await _api.Login(request);

                _jwt.token = token;
                return RedirectToAction("Index", "Home");
            }

            return View(request);
            

            //if (ModelState.IsValid)
            //{
            //    var content = JsonSerializer.Serialize(request);
            //    StringContent stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            //    HttpResponseMessage response = await _httpClient.PostAsync("login", stringContent);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        return RedirectToAction("Index", "Home");
            //    }
            //}

            //return BadRequest();
        }
    }
}
