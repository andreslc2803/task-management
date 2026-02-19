using TaskManagement.DAL.Repositories;
using TaskManagement.Entities.Models;

namespace TaskManagement.BL.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Tasks>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Tasks?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Tasks> CreateAsync(Tasks task)
        {
            task.CreatedAt = DateTime.UtcNow;
            return await _repository.CreateAsync(task);
        }

        public async Task UpdateAsync(int id, Tasks task)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException("Task not found");

            // Update fields
            existing.Title = task.Title;
            existing.Description = task.Description;
            existing.Status = task.Status;
            existing.TaskPriority = task.TaskPriority;

            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException("Task not found");

            await _repository.DeleteAsync(existing);
        }
    }
}
