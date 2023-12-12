using Refit;
using System.Net;
using System.Runtime.InteropServices;
using To_Do_List.API.Models;

namespace To_Do_List.Web
{
    public interface IToDoApi
    {
        [Post("/login")]
        Task<string> Login([Body] UserDTO request);

        [Post("/register")]
        Task<ApiResponse<HttpStatusCode>> Register([Body] UserDTO request);

        [Get("/tasks")]
        Task<ApiResponse<IEnumerable<TaskDTO>>> GetTasks([Authorize("Bearer")] string token);

        [Post("/tasks")]
        Task<ApiResponse<bool>> CreateTask([Body] TaskDTO task, [Authorize("Bearer")] string token);

        [Delete("/tasks/{id}")]
        Task<ApiResponse<HttpStatusCode>> DeleteTask([Body] string id, [Authorize("Bearer")] string token);

        [Put("/tasks")]
        Task<ApiResponse<HttpStatusCode>> EditTask(string id, [Body] TaskDTO task, [Authorize("Bearer")] string token);
    }
}
