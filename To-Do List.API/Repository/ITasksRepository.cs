using To_Do_List.API.Models;

namespace To_Do_List.API.Repository
{
    public interface ITasksRepository
    {
        Task<bool> CreateTaskFromDTO(TaskDTO taskDTO, Guid userId);
        Task<IEnumerable<TaskDTO>> GetUserTasks(Guid userId);
        Task<TaskDTO?> GetUserTaskById(Guid taskId);
        Task<bool> DeleteTask(Guid userId);
        Task<bool> UpdateTask(Guid taskId, TaskDTO taskDTO);

    }
}
