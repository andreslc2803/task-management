using TaskManagement.Entities.Models;
using TaskManagement.Entities.DTO;

namespace TaskManagement.DAL.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for Task persistence operations.
    /// Read operations return DTOs for use by the business layer and controllers.
    /// Create/Update/Delete operate on the data model types.
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// Retrieve all tasks as DTOs.
        /// </summary>
        Task<IEnumerable<TaskDto>> GetAllAsync();

        /// <summary>
        /// Retrieve a single task as DTO by id.
        /// </summary>
        Task<TaskDto?> GetByIdAsync(int id);

        // Model-level operations for create/update/delete
        /// <summary>
        /// Create a new task model in the database.
        /// Returns the created model with identity populated.
        /// </summary>
        Task<Tasks> CreateAsync(Tasks task);

        /// <summary>
        /// Get the data model by id for update/delete operations.
        /// </summary>
        Task<Tasks?> GetModelByIdAsync(int id);

        /// <summary>
        /// Update an existing task model.
        /// </summary>
        Task UpdateAsync(Tasks task);
    }
}
