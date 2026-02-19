using TaskManagement.Entities.Models;

namespace TaskManagement.DAL.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Tasks>> GetAllAsync();
        Task<Tasks?> GetByIdAsync(int id);
        Task<Tasks> CreateAsync(Tasks task);
        Task UpdateAsync(Tasks task);
        Task DeleteAsync(Tasks task);
    }
}
