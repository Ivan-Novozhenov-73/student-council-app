using System;

namespace StudentCouncilAPI.DTOs
{
    public class MeetingDto
    {
        public required Guid MeetingId { get; set; }
        public required string Title { get; set; }
        public required DateTime MeetingDate { get; set; }
        public required TimeSpan MeetingTime { get; set; }
        public required string Location { get; set; }
        public string? Link { get; set; }
    }

    public class CreateMeetingDto
    {
        public string Title { get; set; }
        public DateTime MeetingDate { get; set; }
        public TimeSpan MeetingTime { get; set; }
        public string Location { get; set; }
        public string? Link { get; set; }
    }

    public class UpdateMeetingDto
    {
        public string? Title { get; set; }
        public DateTime? MeetingDate { get; set; }
        public TimeSpan? MeetingTime { get; set; }
        public string? Location { get; set; }
        public string? Link { get; set; }
    }
}