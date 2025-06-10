using System;

namespace StudentCouncilAPI.DTOs
{
    public class EventDto
    {
        public required Guid EventId { get; set; }
        public required string Title { get; set; }
        public required int Status { get; set; }
        public required string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? EventTime { get; set; }
        public string? Location { get; set; }
        public int? NumberOfParticipants { get; set; }
    }

    public class CreateEventDto
    {
        public required string Title { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? EventTime { get; set; }
        public string? Location { get; set; }
        public int? NumberOfParticipants { get; set; }
    }

    public class UpdateEventDto
    {
        public string? Title { get; set; }
        public int? Status { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? EventTime { get; set; }
        public string? Location { get; set; }
        public int? NumberOfParticipants { get; set; }
    }
}