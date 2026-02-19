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
        /// Update an existing task identified by id using the update DTO.
        /// </summary>
        Task UpdateAsync(int id, TaskUpdateDto dto);

        /// <summary>
        /// Delete a task by id.
        /// </summary>
        Task DeleteAsync(int id);
    }
}
