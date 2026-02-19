using TaskManagement.BL.Interfaces;
using TaskManagement.DAL.Repositories.Interfaces;
using TaskManagement.Entities.Models;
using TaskManagement.Entities.DTO;

namespace TaskManagement.BL.Services
{
    /// <summary>
    /// Business service for task operations. Maps between DTOs and data models
    /// and delegates persistence to repository layer.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all tasks as DTOs.
        /// </summary>
        public async Task<IEnumerable<TaskDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Get task by id as DTO.
        /// </summary>
        public async Task<TaskDto?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Create a new task from DTO.
        /// </summary>
        public async Task<TaskDto> CreateAsync(TaskCreateDto dto)
        {
            var task = new Tasks
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                UserId = dto.UserId,
                TaskPriority = dto.TaskPriority,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.CreateAsync(task);
            return new TaskDto
            {
                Id = created.Id,
                Title = created.Title,
                Status = created.Status,
                TaskPriority = created.TaskPriority ?? string.Empty
            };
        }

        /// <summary>
        /// Update existing task from DTO.
        /// </summary>
        public async Task UpdateAsync(int id, TaskUpdateDto dto)
        {
            var existing = await _repository.GetModelByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException("Task not found");

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.Status = dto.Status;
            existing.TaskPriority = dto.TaskPriority;
            existing.UserId = dto.UserId;

            await _repository.UpdateAsync(existing);
        }

        /// <summary>
        /// Delete task by id.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var existing = await _repository.GetModelByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException("Task not found");

            await _repository.DeleteAsync(existing);
        }
    }
}
