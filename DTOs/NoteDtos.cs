using System;

namespace StudentCouncilAPI.DTOs
{
    public class NoteDto
    {
        public required Guid NoteId { get; set; }
        public required Guid UserId { get; set; }
        public required Guid EventId { get; set; }
        public required string Title { get; set; }
        public required string FilePath { get; set; }
    }

    public class CreateNoteDto
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
    }

    public class UpdateNoteDto
    {
        public string? Title { get; set; }
        public string? FilePath { get; set; }
    }
}