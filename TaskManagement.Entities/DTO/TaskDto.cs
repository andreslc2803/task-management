using TaskManagement.Entities.Constants;

namespace TaskManagement.Entities.DTO
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string TaskPriority { get; set;  } = string.Empty;
    }

    public class TaskUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string? TaskPriority { get; set; } 
    }

    public class TaskCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string? TaskPriority { get; set; } = TaskStatuses.Pending;
    }
}
