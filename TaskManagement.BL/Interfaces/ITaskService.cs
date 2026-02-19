using TaskManagement.Entities.DTO;

namespace TaskManagement.BL.Interfaces
{
    public interface ITaskService
    {
        /// <summary>
        /// Retrieve all tasks as DTOs for API responses.
        /// </summary>
        Task<IEnumerable<TaskDto>> GetAllAsync();

        /// <summary>
        /// Retrieve a single task as DTO by id for API responses.
        /// </summary>
        Task<TaskDto?> GetByIdAsync(int id);

        /// <summary>
        /// Create a new task from a creation DTO and return the created DTO.
        /// </summary>
        Task<TaskDto> CreateAsync(TaskCreateDto dto);

        /// <summary>
        /// Validate json format
        /// </summary>
        bool IsValidJson(string json);

        /// <summary>
        /// Update only the status of a task with business rules.
        /// </summary>
        Task UpdateStatusAsync(int id, string newStatus);
    }
}
