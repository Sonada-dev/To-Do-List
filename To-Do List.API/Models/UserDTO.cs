using System.ComponentModel.DataAnnotations;

namespace To_Do_List.API.Models
{
    public class UserDTO
    {
        [Required, Display(Name = "Логин")]
        public string Username { get; set; } = string.Empty;
        [Required, Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;
    }
}
