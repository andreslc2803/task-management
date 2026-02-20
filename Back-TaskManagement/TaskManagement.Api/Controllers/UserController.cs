using Microsoft.AspNetCore.Mvc;
using TaskManagement.BL.Interfaces;
using TaskManagement.Entities.DTO;

namespace TaskManagement.Api.Controllers
{
    /// <summary>
    /// Controller to manage User.
    /// Provides endpoints to create, read, update and delete user.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        /// <summary>
        /// Constructor that injects the User service.
        /// </summary>
        /// <param name="service">Business service for user.</param>
        public UserController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>List of Users.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(CancellationToken cancellationToken = default)
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        /// <summary>
        /// Retrieves a specific user by id.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <returns>The user DTO if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUser(int id, CancellationToken cancellationToken = default)
        {
            var user = await _service.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userDto">User create DTO.</param>
        /// <returns>Created user DTO with its ID.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserCreateDto userDto, CancellationToken cancellationToken = default)
        {
            if (userDto == null)
                return BadRequest("User payload is required.");

            if (string.IsNullOrEmpty(userDto.Name))
                return BadRequest("Name is required.");

            if (string.IsNullOrWhiteSpace(userDto.Email))
                return BadRequest("Email is required.");

            if (!_service.IsValidEmail(userDto.Email))
                return BadRequest("Email format is invalid.");

            var created = await _service.CreateAsync(userDto);
            return CreatedAtAction(nameof(GetUser), new { id = created.Id }, created);
        }
    }
}
