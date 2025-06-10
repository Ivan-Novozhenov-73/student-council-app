using System;

namespace StudentCouncilAPI.DTOs
{
    public class TaskDto
    {
        public required Guid TaskId { get; set; }
        public required Guid EventId { get; set; }
        public Guid? PartnerId { get; set; }
        public required string Title { get; set; }
        public required int Status { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
    }

    public class CreateTaskDto
    {
        public Guid EventId { get; set; }
        public Guid? PartnerId { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class UpdateTaskDto
    {
        public Guid? PartnerId { get; set; }
        public string? Title { get; set; }
        public int? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}