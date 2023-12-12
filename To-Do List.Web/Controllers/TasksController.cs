using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using To_Do_List.API.Models;
using To_Do_List.Web.Models;

namespace To_Do_List.Web.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly IToDoApi _api;

        public TasksController(IToDoApi api) => _api = api;


        public async Task<IActionResult> Index(TaskDTO? task = null)
        {
            var response = await _api.GetTasks(Request.Headers.Authorization.ToString().Split()[1]);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Account");
            else if (!response.IsSuccessStatusCode)
                return NotFound();

            ViewData["task"] = task;

            return View(response.Content);
        }

        [HttpPost("Create")]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask([FromForm] TaskDTO task)
        {
            if (ModelState.IsValid)
            {
                var response = await _api.CreateTask(task, Request.Headers.Authorization.ToString().Split()[1]);
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return RedirectToAction("Login", "Account");
                else if (!response.IsSuccessStatusCode)
                    return BadRequest();
            }

            return RedirectToAction("Index");
        }

        [HttpPost("Delete/{id}")]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            var response = await _api.DeleteTask(id, Request.Headers.Authorization.ToString().Split()[1]);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Account");
            else if (!response.IsSuccessStatusCode)
                return BadRequest();

            return RedirectToAction("Index");
        }

        [HttpPost("Edit/{id}")]
        [ActionName("Edit")]
        public async Task<IActionResult> EditTask(string id, [FromForm] TaskDTO task)
        {
            var response = await _api.EditTask(id, task, Request.Headers.Authorization.ToString().Split()[1]);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Account");
            else if (!response.IsSuccessStatusCode)
                return BadRequest();

            return RedirectToAction("Index");
        }
    }
}
