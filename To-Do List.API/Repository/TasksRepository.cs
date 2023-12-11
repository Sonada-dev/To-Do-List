using Microsoft.EntityFrameworkCore;
using System.Reflection;
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

        public async Task<bool> CreateTaskFromDTO(TaskDTO taskDTO, Guid userId)
        {
            try
            {
                var task = new ToDoTask
                {
                    Title = taskDTO.Title,
                    Description = taskDTO.Description,
                    Deadline = taskDTO.Deadline.ToUniversalTime(),
                    Priority = EnumExtensions.ParseFromDisplayName<Priority>(taskDTO.Priority, ignoreCase: true),
                    Status =  EnumExtensions.ParseFromDisplayName<Models.TaskStatus>(taskDTO.Status, ignoreCase: true),
                    UserId = userId
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Task created with Id: {task.Id} for User Id: {userId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating task: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> DeleteTask(Guid taskId)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(taskId);
                if (task == null)
                {
                    _logger.LogWarning($"Task with Id {taskId} not found");
                    return false;
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Task with Id: {taskId} deleted");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting task: {ex.Message}");
                return false;
            }
        }

        public async Task<TaskDTO?> GetUserTaskById(Guid taskId)
        {
            try
            {
                var task = await _context.Tasks
                    .FirstOrDefaultAsync(t => t.Id == taskId);

                if (task == null)
                {
                    _logger.LogWarning($"Task with ID {taskId} not found.");
                    return null;
                }

                var taskDTO = new TaskDTO
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Deadline = task.Deadline,
                    Priority = task.Priority.GetEnumDisplayName(),
                    Status = task.Status.GetEnumDisplayName()
                };

                return taskDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while retrieving task with ID {taskId}: {ex.Message}");
                return null;
            }
        }


        public async Task<IEnumerable<TaskDTO>> GetUserTasks(Guid userId)
        {
            try
            {
                var tasks = await _context.Tasks
                    .Where(task => task.UserId == userId)
                    .Select(task => new TaskDTO
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        Deadline = task.Deadline,
                        Priority = task.Priority.GetEnumDisplayName(),
                        Status = task.Status.GetEnumDisplayName(),
                    })
                    .ToListAsync();

                _logger.LogInformation($"Retrieved {tasks.Count} tasks for User Id: {userId}");
                return tasks;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving tasks: {ex.Message}");
                return Enumerable.Empty<TaskDTO>();
            }
        }

        public async Task<bool> UpdateTask(Guid taskId, TaskDTO taskDTO)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(taskId);
                if (task == null)
                {
                    _logger.LogWarning($"Task with Id {taskId} not found");
                    return false;
                }

                task.Title = taskDTO.Title;
                task.Description = taskDTO.Description;
                task.Deadline = taskDTO.Deadline.ToUniversalTime();
                task.Priority = EnumExtensions.ParseFromDisplayName<Priority>(taskDTO.Priority, ignoreCase: true);
                task.Status = EnumExtensions.ParseFromDisplayName<Models.TaskStatus>(taskDTO.Status, ignoreCase: true);

                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Task with Id: {taskId} updated");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating task: {ex.Message}");
                return false;
            }
        }
    }
}
