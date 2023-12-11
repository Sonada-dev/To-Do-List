using Refit;
using System.Runtime.InteropServices;
using To_Do_List.API.Models;

namespace RefitInterface
{
    public interface IToDoApi
    {
        [Post("/login")]
        Task<string> Login([Body] UserDTO request);

        [Get("/tasks")]
        Task<IEnumerable<TaskDTO>> GetTasks();

        [Post("/tasks")]
        Task CreateTask([Body] TaskDTO task);
    }
}
