using System.ComponentModel.DataAnnotations;

namespace StudentCouncilAPI.Models
{
    public class Partner
    {
        [Key]
        public Guid PartnerId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Surname { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Patronymic { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public long Phone { get; set; }

        [Required]
        [MaxLength(255)]
        public string Contacts { get; set; } = string.Empty;

        [Required]
        public bool Archive { get; set; } = false;

        // Navigation properties
        public virtual ICollection<EventPartner> EventPartners { get; set; } = new List<EventPartner>();
        public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
