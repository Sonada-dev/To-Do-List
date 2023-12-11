using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace To_Do_List.API.Models
{
    public class ToDoTask
    {
        public ToDoTask() => Id = Guid.NewGuid();

        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Deadline { get; set; }

        [Column(TypeName = "int")]
        [EnumDataType(typeof(Priority))]
        public Priority Priority { get; set; }

        [Column(TypeName = "int")]
        [EnumDataType(typeof(TaskStatus))]
        public TaskStatus Status { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }

    public enum Priority
    {
        [Display(Name = "Низкий")]
        Low = 1,
        [Display(Name = "Средний")]
        Medium = 2,
        [Display(Name = "Высокий")]
        High = 3
    }

    public enum TaskStatus //использовал enum, а не bool для возможного добавления новых статусов задачи в будущем. (ну типа "В процессе", "Не начато" и т.д.")
    {
        [Display(Name = "Не выполнено")]
        NotCompleted = 0,
        [Display(Name = "Выполнено")]
        Completed = 1
    }
}
