using System.ComponentModel.DataAnnotations;

namespace StudentCouncilAPI.Models
{
    public class Event
    {
        [Key]
        public Guid EventId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public EventStatus Status { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public TimeSpan? EventTime { get; set; }

        [MaxLength(255)]
        public string? Location { get; set; }

        public int? NumberOfParticipants { get; set; }

        // Navigation properties
        public virtual ICollection<EventUser> EventUsers { get; set; } = new List<EventUser>();
        public virtual ICollection<EventPartner> EventPartners { get; set; } = new List<EventPartner>();
        public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
        public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
    }

    public enum EventStatus
    {
        Incoming = 0,
        Planning = 1,
        InProgress = 2,
        Paused = 3,
        Completed = 4,
        Cancelled = 5
    }
}
