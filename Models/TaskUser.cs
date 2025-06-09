using System.ComponentModel.DataAnnotations;

namespace StudentCouncilAPI.Models
{
    public class TaskUser
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid TaskId { get; set; }

        [Required]
        public TaskUserRole Role { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Task Task { get; set; } = null!;
    }

    public enum TaskUserRole
    {
        Assignee = 0,
        Assigner = 1
    }
}
