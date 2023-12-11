using Microsoft.AspNetCore.Mvc;
using RefitInterface;
using To_Do_List.API.Models;

namespace To_Do_List.Web.Controllers
{
    public class TasksController : Controller
    {
        private readonly IToDoApi _api;

        public TasksController(IToDoApi api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _api.GetTasks());
        }

        [HttpGet("{controller}/Create")]
        public IActionResult CreateTask()
        {
            return View("Create", new TaskDTO());
        }

        [HttpPost("{controller}/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask([FromForm] TaskDTO task)
        {
            if (ModelState.IsValid)
            {
                await _api.CreateTask(task);
                return RedirectToAction("Index");
            }

            return View("Create",task);
        }
    }
}
