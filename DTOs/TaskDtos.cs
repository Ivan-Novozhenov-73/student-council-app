using System;

namespace StudentCouncilAPI.DTOs
{
    public class TaskDto
    {
        public Guid TaskId { get; set; }
        public Guid EventId { get; set; }
        public Guid? PartnerId { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
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