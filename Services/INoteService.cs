using StudentCouncilAPI.Models;

namespace StudentCouncilAPI.Services
{
    public interface INoteService
    {
        Task<IEnumerable<Note>> GetAllNotesAsync();
        Task<Note?> GetNoteByIdAsync(Guid id);
        Task<Note> CreateNoteAsync(Note note);
        Task<Note> UpdateNoteAsync(Note note);
        Task<bool> DeleteNoteAsync(Guid id);
        Task<IEnumerable<Note>> GetNotesByUserAsync(Guid userId);
        Task<IEnumerable<Note>> GetNotesByEventAsync(Guid eventId);
        Task<string> GetNoteContentAsync(Guid noteId);
        Task<bool> SaveNoteContentAsync(Guid noteId, string content);
    }
}
