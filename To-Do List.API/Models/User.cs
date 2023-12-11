using System.ComponentModel.DataAnnotations;

namespace To_Do_List.API.Models
{
    public class User
    {
        public User() => Id = Guid.NewGuid();

        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires{ get; set; }
        public List<ToDoTask> Tasks { get; set; } = new List<ToDoTask>();
    }
}
