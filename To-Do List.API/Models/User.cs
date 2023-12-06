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
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}
