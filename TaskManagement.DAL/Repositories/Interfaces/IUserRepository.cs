using TaskManagement.Entities.DTO;
using TaskManagement.Entities.Models;

namespace TaskManagement.DAL.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for User persistence operations.
    /// Read operations return DTOs for API consumption. Create/Update/Delete
    /// operate on the data model types.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieve all users as DTOs (including tasks).
        /// </summary>
        Task<IEnumerable<UserDto>> GetAllAsync();

        /// <summary>
        /// Retrieve a single user as DTO by id.
        /// </summary>
        Task<UserDto?> GetByIdAsync(int id);

        /// <summary>
        /// Create a new user model in the database.
        /// </summary>
        Task<User> CreateAsync(User user);

        /// <summary>
        /// Get the data model by id for update/delete operations.
        /// </summary>
        Task<User?> GetModelByIdAsync(int id);
    }
}
