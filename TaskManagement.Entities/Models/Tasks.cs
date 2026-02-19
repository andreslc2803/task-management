using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Entities.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Status { get; set; } = null!;
        public User User { get; set; } = null!;
        public int UserId { get; set; }
        public string? TaskPriority { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
