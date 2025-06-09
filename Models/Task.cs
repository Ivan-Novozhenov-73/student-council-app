using System.ComponentModel.DataAnnotations;

namespace StudentCouncilAPI.Models
{
    public class Task
    {
        [Key]
        public Guid TaskId { get; set; }

        [Required]
        public Guid EventId { get; set; }

        public Guid? PartnerId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public TaskStatus Status { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        // Navigation properties
        public virtual Event Event { get; set; } = null!;
        public virtual Partner? Partner { get; set; }
        public virtual ICollection<TaskUser> TaskUsers { get; set; } = new List<TaskUser>();
    }

    public enum TaskStatus
    {
        Todo = 0,
        InProgress = 1,
        Review = 2,
        Done = 3,
        Cancelled = 4
    }
}
