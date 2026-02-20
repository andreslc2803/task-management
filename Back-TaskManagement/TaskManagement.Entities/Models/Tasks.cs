namespace TaskManagement.Entities.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public string? TaskPriority { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
