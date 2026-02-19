using System.Text.Json;
using TaskManagement.BL.Interfaces;
using TaskManagement.DAL.Repositories.Interfaces;
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
        /// Update only the status of a task with business rules.
        /// </summary>
        public async Task UpdateStatusAsync(int id, string newStatus)
        {
            var task = await _repositoryTask.GetModelByIdAsync(id);

            if (task == null)
                throw new KeyNotFoundException("Task not found");

            if (task.Status == Entities.Constants.TaskStatus.Pending && newStatus == Entities.Constants.TaskStatus.Done)
                throw new InvalidOperationException("Cannot change status from Pending directly to Done");

            task.Status = newStatus;
            await _repositoryTask.UpdateAsync(task);
        }

        /// <summary>
        /// Validate json format
        /// </summary>
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
