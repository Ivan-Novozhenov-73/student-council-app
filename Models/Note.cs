using System.ComponentModel.DataAnnotations;

namespace StudentCouncilAPI.Models
{
    public class Note
    {
        [Key]
        public Guid NoteId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid EventId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string FilePath { get; set; } = string.Empty;

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Event Event { get; set; } = null!;
    }
}
