using System.Text.Json;
using TaskManagement.BL.Interfaces;
using TaskManagement.DAL.Repositories.Interfaces;
using TaskManagement.Entities.Constants;
using TaskManagement.Entities.DTO;
using TaskManagement.Entities.Models;

namespace TaskManagement.BL.Services
{
    /// <summary>
    /// Business service for task operations. Maps between DTOs and data models
    /// and delegates persistence to repository layer.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repositoryTask;
        private readonly IUserRepository _repositoryUser;

        public TaskService(ITaskRepository repository, IUserRepository repositoryUser)
        {
            _repositoryTask = repository;
            _repositoryUser = repositoryUser;
        }

        /// <summary>
        /// Get all tasks as DTOs.
        /// </summary>
        public async Task<IEnumerable<TaskDto>> GetAllAsync()
        {
            return await _repositoryTask.GetAllAsync();
        }

        /// <summary>
        /// Get task by id as DTO.
        /// </summary>
        public async Task<TaskDto?> GetByIdAsync(int id)
        {
            return await _repositoryTask.GetByIdAsync(id);
        }

        /// <summary>
        /// Create a new task from DTO.
        /// </summary>
        public async Task<TaskDto> CreateAsync(TaskCreateDto dto)
        {
            var userExists = await _repositoryUser.GetModelByIdAsync(dto.UserId);
            if (userExists == null)
                throw new InvalidOperationException("Assigned user does not exist");

            var task = new Tasks
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                UserId = dto.UserId,
                TaskPriority = dto.TaskPriority,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repositoryTask.CreateAsync(task);
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
            var existing = await _repositoryTask.GetModelByIdAsync(id);
            
            if (existing == null) 
                throw new KeyNotFoundException("Task not found");

            // Rule: cannot change directly from Pending->Done
            if (existing.Status == TaskStatuses.Pending && dto.Status == TaskStatuses.Done)
                throw new InvalidOperationException("Cannot change status from Pending directly to Done");

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.Status = dto.Status;
            existing.TaskPriority = dto.TaskPriority;
            existing.UserId = dto.UserId;

            await _repositoryTask.UpdateAsync(existing);
        }

        /// <summary>
        /// Delete task by id.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var existing = await _repositoryTask.GetModelByIdAsync(id);

            if (existing == null) 
                throw new KeyNotFoundException("Task not found");

            await _repositoryTask.DeleteAsync(existing);
        }

        /// <summary>
        /// Update only the status of a task with business rules.
        /// </summary>
        public async Task UpdateStatusAsync(int id, string newStatus)
        {
            var task = await _repositoryTask.GetModelByIdAsync(id);

            if (task == null)
                throw new KeyNotFoundException("Task not found");

            if (task.Status == TaskStatuses.Pending && newStatus == TaskStatuses.Done)
                throw new InvalidOperationException("Cannot change status from Pending directly to Done");

            task.Status = newStatus;
            await _repositoryTask.UpdateAsync(task);
        }

        public bool IsValidJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return false;

            try
            {
                JsonDocument.Parse(json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
