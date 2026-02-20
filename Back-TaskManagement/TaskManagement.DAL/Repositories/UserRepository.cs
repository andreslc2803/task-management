using Microsoft.EntityFrameworkCore;
using TaskManagement.DAL.Data;
using TaskManagement.DAL.Repositories.Interfaces;
using TaskManagement.Entities.DTO;
using TaskManagement.Entities.Models;

namespace TaskManagement.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskManagementDbContext _context;

        public UserRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        // GET: all users with tasks as DTO
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _context.User
                .Include(u => u.Tasks)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    CreateAt = u.CreatedAt,
                    Tasks = u.Tasks.Select(t => new TaskDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Status = t.Status,
                        TaskPriority = t.TaskPriority
                    }).ToList()
                })
                .AsNoTracking()
                .ToListAsync();
        }

        // GET: user by Id
        public async Task<UserDto?> GetByIdAsync(int id)
        {
            return await _context.User
                .Include(u => u.Tasks)
                .AsNoTracking()
                .Where(u => u.Id == id)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    CreateAt = u.CreatedAt,
                    Tasks = u.Tasks.Select(t => new TaskDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Status = t.Status,
                        TaskPriority = t.TaskPriority
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        // POST: create
        public async Task<User> CreateAsync(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetModelByIdAsync(int id)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
