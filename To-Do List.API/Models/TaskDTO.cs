namespace To_Do_List.API.Models
{
    public class TaskDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Deadline { get; set; }
        public Priority Priority { get; set; }
        public TaskStatus Status { get; set; }
    }

}
