using System.ComponentModel.DataAnnotations;
using TaskManagement.Entities.Constants;
using System.Text.Json;

namespace TaskManagement.Entities.DTO
{
    public class TaskDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty;

        public string TaskPriority { get; set; } = string.Empty;
    }

    public class TaskCreateDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200, ErrorMessage = "Title can have at most 400 characters.")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [MaxLength(20)]
        [RegularExpression("Pending|InProgress|Done", ErrorMessage = "Status must be Pending, InProgress, or Done.")]
        public string Status { get; set; } = Constants.TaskStatus.Pending;

        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        public string? TaskPriority { get; set; }
    }
}
