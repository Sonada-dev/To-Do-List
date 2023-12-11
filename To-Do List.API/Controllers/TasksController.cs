using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using To_Do_List.API.Models;
using To_Do_List.API.Repository;

namespace To_Do_List.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksRepository _tasksRepository;

        public TasksController(ITasksRepository tasksRepository)
        {
            _tasksRepository = tasksRepository;
        }

        [Authorize]
        [HttpPost("tasks")]
        public async Task<IActionResult> CreateTask([FromBody]TaskDTO taskDTO)
        {
            var userId = Guid.Parse("52c201b7-0afe-4766-bbf1-d54fae3044c4");//Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var task = await _tasksRepository.CreateTaskFromDTO(taskDTO, userId);
            if(!task)
                return BadRequest();

            return Ok(task);
        }

        [Authorize]
        [HttpGet("tasks")]
        public async Task<IActionResult> GetTasks()
        {
            var userId = Guid.Parse("52c201b7-0afe-4766-bbf1-d54fae3044c4"); //Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var tasks = await _tasksRepository.GetUserTasks(userId);
            if (tasks == null)
                return NotFound();

            return Ok(tasks);
        }

        [Authorize]
        [HttpDelete("tasks/{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            if(Guid.TryParse(id, out var taskId))
            {
                if(await _tasksRepository.DeleteTask(taskId))
                    return NoContent();
            }
           
            return NotFound();
        }

        [Authorize]
        [HttpPut("tasks")]
        public async Task<IActionResult> UpdateTask(string id, [FromBody] TaskDTO taskDTO)
        {
            if (Guid.TryParse(id, out var taskId))
            {
                if (await _tasksRepository.UpdateTask(taskId, taskDTO))
                    return Ok("Task updated");
            }

            return BadRequest();
        }

        [Authorize]
        [HttpGet("tasks/{id}")]
        public async Task<IActionResult> GetTaskById(string id)
        {
            if (Guid.TryParse(id, out var taskId))
            {
                var task = await _tasksRepository.GetUserTaskById(taskId);
                if(task != null)
                    return Ok(task);
            }

            return NotFound();
        }
    }
}
