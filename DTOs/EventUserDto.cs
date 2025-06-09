namespace StudentCouncilAPI.DTOs
{
    public class EventUserDto
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; }
    }
}