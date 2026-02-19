using Microsoft.AspNetCore.Mvc;
using TaskManagement.BL.Interfaces;
using TaskManagement.Entities.DTO;

namespace TaskManagement.Api.Controllers
{
    /// <summary>
    /// Controller to manage Tasks.
    /// Provides endpoints to create, read, update and delete tasks.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _service;

        /// <summary>
        /// Constructor that injects the Task service.
        /// </summary>
        /// <param name="service">Business service for tasks.</param>
        public TaskController(ITaskService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all tasks.
        /// </summary>
        /// <returns>List of TaskItems.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks()
        {
            var tasks = await _service.GetAllAsync();
            return Ok(tasks);
        }

        /// <summary>
        /// Retrieves a specific task by id.
        /// </summary>
        /// <param name="id">Task ID.</param>
        /// <returns>The task if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(int id)
        {
            var task = await _service.GetByIdAsync(id);

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="task">Task object to create.</param>
        /// <returns>Created task with its ID.</returns>
        [HttpPost]
        public async Task<ActionResult<TaskDto>> CreateTask(TaskCreateDto task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
                return BadRequest("Title is required");

            if (task.UserId <= 0)
                return BadRequest("UserId must be provided and greater than 0");

            if (!_service.IsValidJson(task.TaskPriority))
                return BadRequest("TaskPriority must be valid JSON");

            var created = await _service.CreateAsync(task);
            return CreatedAtAction(nameof(GetTask), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates only the status of a task.
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
        {
            var validStatuses = new[]
            {
                Entities.Constants.TaskStatus.Pending,
                Entities.Constants.TaskStatus.InProgress,
                Entities.Constants.TaskStatus.Done
            };

            if (string.IsNullOrWhiteSpace(newStatus) || !validStatuses.Contains(newStatus))
                return BadRequest($"Invalid status value. Valid values are: {string.Join(", ", validStatuses)}");

            try
            {
                await _service.UpdateStatusAsync(id, newStatus);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
    }
}
