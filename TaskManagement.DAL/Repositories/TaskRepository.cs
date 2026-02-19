using Microsoft.EntityFrameworkCore;
using TaskManagement.DAL.Data;
using TaskManagement.DAL.Repositories.Interfaces;
using TaskManagement.Entities.Models;
using TaskManagement.Entities.DTO;

namespace TaskManagement.DAL.Repositories
{
    /// <summary>
    /// Repository implementation for task persistence. Maps data model to DTOs for read operations
    /// and provides model-level methods for create/update/delete.
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementDbContext _context;

        public TaskRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskDto>> GetAllAsync()
        {
            return await _context.Tasks
                .AsNoTracking()
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Status = t.Status,
                    TaskPriority = t.TaskPriority ?? string.Empty
                })
                .ToListAsync();
        }

        public async Task<TaskDto?> GetByIdAsync(int id)
        {
            var task = await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (task == null) return null;
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Status = task.Status,
                TaskPriority = task.TaskPriority ?? string.Empty
            };
        }

        public async Task<Tasks?> GetModelByIdAsync(int id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tasks> CreateAsync(Tasks task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task UpdateAsync(Tasks task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tasks task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}