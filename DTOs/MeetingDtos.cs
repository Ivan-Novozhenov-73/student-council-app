using System;

namespace StudentCouncilAPI.DTOs
{
    public class MeetingDto
    {
        public Guid MeetingId { get; set; }
        public string Title { get; set; }
        public DateTime MeetingDate { get; set; }
        public TimeSpan MeetingTime { get; set; }
        public string Location { get; set; }
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