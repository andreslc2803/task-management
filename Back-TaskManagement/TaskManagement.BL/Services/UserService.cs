using System.Text.RegularExpressions;
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
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all users as DTOs.
        /// </summary>
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Get user by id as DTO.
        /// </summary>
        public async Task<UserDto?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
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
                CreatedAt = DateTime.Now
            };

            var created = await _repository.CreateAsync(user);
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
        /// Validate email format using a regular expression. Returns true if valid, false otherwise.
        /// </summary>
        public bool IsValidEmail(string email)
        {
            // Simple regex for email validation
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }
    }
}
