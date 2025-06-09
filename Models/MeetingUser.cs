using System.ComponentModel.DataAnnotations;

namespace StudentCouncilAPI.Models
{
    public class MeetingUser
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid MeetingId { get; set; }

        [Required]
        public MeetingUserRole Role { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Meeting Meeting { get; set; } = null!;
    }

    public enum MeetingUserRole
    {
        Participant = 0,
        Organizer = 1
    }
}
