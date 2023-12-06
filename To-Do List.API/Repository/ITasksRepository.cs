using To_Do_List.API.Models;

namespace To_Do_List.API.Repository
{
    public interface ITasksRepository
    {
        System.Threading.Tasks.Task<Models.Task> CreateTaskFromDTO(TaskDTO taskDTO, Guid userId);
        System.Threading.Tasks.Task<List<Models.Task>> GetUserTasks(Guid userId);
        System.Threading.Tasks.Task<bool> DeleteTask(Guid userId);

    }
}
