using System.ComponentModel.DataAnnotations;

namespace StudentCouncilAPI.Models
{
    public class EventPartner
    {
        [Required]
        public Guid PartnerId { get; set; }

        [Required]
        public Guid EventId { get; set; }

        // Navigation properties
        public virtual Partner Partner { get; set; } = null!;
        public virtual Event Event { get; set; } = null!;
    }
}
