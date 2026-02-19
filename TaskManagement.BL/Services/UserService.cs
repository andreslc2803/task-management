using TaskManagement.BL.Interfaces;
using TaskManagement.DAL.Repositories.Interfaces;
using TaskManagement.Entities.DTO;
using TaskManagement.Entities.Models;

namespace TaskManagement.BL.Services
{
    /// <summary>
    /// Business service for user operations. Maps between DTOs and data models
    /// and delegates persistence to repository layer.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repository)
        {
            _repo = repository;
        }

        /// <summary>
        /// Get all users as DTOs.
        /// </summary>
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        /// <summary>
        /// Get user by id as DTO.
        /// </summary>
        public async Task<UserDto?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        /// <summary>
        /// Create a new user from DTO.
        /// </summary>
        public async Task<UserDto> CreateAsync(UserCreateDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repo.CreateAsync(user);
            return new UserDto
            {
                Id = created.Id,
                Name = created.Name,
                Email = created.Email,
                CreateAt = created.CreatedAt,
                Tasks = new List<TaskDto>()
            };
        }

        /// <summary>
        /// Update existing user from DTO.
        /// </summary>
        public async Task UpdateAsync(int id, UserUpdateDto dto)
        {
            var existing = await _repo.GetModelByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException("User not found");

            existing.Name = dto.Name;
            existing.Email = dto.Email;

            await _repo.UpdateAsync(existing);
        }

        /// <summary>
        /// Delete user by id.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var existing = await _repo.GetModelByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException("User not found");

            await _repo.DeleteAsync(existing);
        }
    }
}
