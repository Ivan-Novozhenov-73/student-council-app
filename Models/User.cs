using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCouncilAPI.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Surname { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Patronymic { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required]
        [MaxLength(12)]
        public string Group { get; set; } = string.Empty;

        [Required]
        public long Phone { get; set; }

        [Required]
        [MaxLength(255)]
        public string Contacts { get; set; } = string.Empty;

        [Required]
        public bool Archive { get; set; } = false;

        // Navigation properties
        public virtual ICollection<EventUser> EventUsers { get; set; } = new List<EventUser>();
        public virtual ICollection<TaskUser> TaskUsers { get; set; } = new List<TaskUser>();
        public virtual ICollection<MeetingUser> MeetingUsers { get; set; } = new List<MeetingUser>();
        public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
    }

    public enum UserRole
    {
        Activist = 0,
        Head = 1,
        Chairman = 2
    }
}
