using System.ComponentModel.DataAnnotations;

namespace StudentCouncilAPI.Models
{
    public class EventUser
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid EventId { get; set; }

        [Required]
        public EventUserRole Role { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Event Event { get; set; } = null!;
    }

    public enum EventUserRole
    {
        Participant = 0,
        Organizer = 1,
        MainOrganizer = 2
    }
}
