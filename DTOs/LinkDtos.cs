using System;

namespace StudentCouncilAPI.DTOs
{
    public class EventUserLinkDto
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public int Role { get; set; }
    }

    public class TaskUserLinkDto
    {
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }
        public int Role { get; set; }
    }

    public class MeetingUserLinkDto
    {
        public Guid UserId { get; set; }
        public Guid MeetingId { get; set; }
        public int Role { get; set; }
    }

    public class EventPartnerLinkDto
    {
        public Guid PartnerId { get; set; }
        public Guid EventId { get; set; }
    }
}