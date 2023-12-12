using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using To_Do_List.API.Models;
using To_Do_List.Web.Models;

namespace To_Do_List.Web.Controllers
{
    public class TasksController : Controller
    {
        private readonly IToDoApi _api;
        private readonly JWT _jwt;

        public TasksController(IToDoApi api, JWT jwt)
        {
            _api = api;
            _jwt = jwt;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _api.GetTasks(_jwt.Token);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Authorize");
            else if (!response.IsSuccessStatusCode)
                return NotFound();

            return View(response.Content);
        }

        [HttpPost("{controller}/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask([FromForm] TaskDTO task)
        {
            if (ModelState.IsValid)
            {
                var response = await _api.CreateTask(task, _jwt.Token);
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return RedirectToAction("Login", "Authorize");
                else if (!response.IsSuccessStatusCode)
                    return BadRequest();
            }

            return RedirectToAction("Index");
        }

        [HttpPost("Delete/{id}")]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            var response = await _api.DeleteTask(id, _jwt.Token);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Authorize");
            else if (!response.IsSuccessStatusCode)
                return BadRequest();

            return RedirectToAction("Index");
        }

        [HttpPost("Edit/{id}")]
        [ActionName("Edit")]
        public async Task<IActionResult> EditTask(string id, [FromForm] TaskDTO task)
        {
            var response = await _api.EditTask(id, task, _jwt.Token);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Authorize");
            else if (!response.IsSuccessStatusCode)
                return BadRequest();

            return RedirectToAction("Index");
        }
    }
}
