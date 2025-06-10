namespace StudentCouncilAPI.DTOs
{
    public class EventUserDto
    {
        public required Guid EventId { get; set; }
        public required Guid UserId { get; set; }
        public required string Role { get; set; }
    }
}