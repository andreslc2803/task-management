using System;
using TaskManagement.Entities.DTO;

namespace TaskManagement.BL.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Retrieve all users as DTOs (including nested tasks).
        /// </summary>
        Task<IEnumerable<UserDto>> GetAllAsync();

        /// <summary>
        /// Retrieve a single user as DTO by id.
        /// </summary>
        Task<UserDto?> GetByIdAsync(int id);

        /// <summary>
        /// Create a new user from create DTO and return created DTO.
        /// </summary>
        Task<UserDto> CreateAsync(UserCreateDto dto);

        /// <summary>
        /// Validate email format using a regular expression. Returns true if valid, false otherwise.
        /// </summary>
        bool IsValidEmail(string email);
    }
}
