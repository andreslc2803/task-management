using TaskManagement.Entities.Models;

namespace TaskManagement.BL.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<Tasks>> GetAllAsync();
        Task<Tasks?> GetByIdAsync(int id);
        Task<Tasks> CreateAsync(Tasks task);
        Task UpdateAsync(int id, Tasks task);
        Task DeleteAsync(int id);
    }
}
