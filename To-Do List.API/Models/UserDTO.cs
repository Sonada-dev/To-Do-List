using System.ComponentModel.DataAnnotations;

namespace To_Do_List.API.Models
{
    public class UserDTO
    {
        [Required(ErrorMessage = "Обязательное поле"), Display(Name = "Логин")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Обязательное поле"), Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;
    }
}
