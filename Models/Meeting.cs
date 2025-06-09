using System.ComponentModel.DataAnnotations;

namespace StudentCouncilAPI.Models
{
    public class Meeting
    {
        [Key]
        public Guid MeetingId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public DateTime MeetingDate { get; set; }

        [Required]
        public TimeSpan MeetingTime { get; set; }

        [Required]
        [MaxLength(255)]
        public string Location { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Link { get; set; }

        // Navigation properties
        public virtual ICollection<MeetingUser> MeetingUsers { get; set; } = new List<MeetingUser>();
    }
}
