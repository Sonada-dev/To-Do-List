using Microsoft.EntityFrameworkCore;
using To_Do_List.API.Data;
using To_Do_List.API.Models;

namespace To_Do_List.API.Repository
{
    public class TasksRepository : ITasksRepository
    {
        private readonly ApiDBContext _context;
        private readonly ILogger<TasksRepository> _logger;

        public TasksRepository(ApiDBContext context, ILogger<TasksRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async System.Threading.Tasks.Task<Models.Task> CreateTaskFromDTO(TaskDTO taskDTO, Guid userId)
        {
            var task = new Models.Task
            {
                Title = taskDTO.Title,
                Description = taskDTO.Description,
                Deadline = taskDTO.Deadline,
                Priority = taskDTO.Priority,
                Status = taskDTO.Status,
                UserId = userId
            };

            _context.Task.Add(task);    
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<bool> DeleteTask(Guid userId)
        {
            var task = await _context.Task.FindAsync(userId);
            if (task == null)
                return false;

            _context.Task.Remove(task);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Models.Task>> GetUserTasks(Guid userId) =>
            await _context.Task.Where(task => task.UserId == userId).ToListAsync();
    }
}
